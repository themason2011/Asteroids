using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float turnSpeed = 50;
    public float speed = 10;
    public float maxSpeed = 20;
    public float maxBulletSpeed = 25;
    public float bulletSpeed = 10;
    public float fireRate = 0.2f;
    public AudioSource Thrusters;
    public AudioSource ShipExplosion;
    public AudioSource AsteroidExplosion;
    public GameObject explosionPrefab;

    private float xPos;
    private float yPos;
    private PolygonCollider2D poly;
    private SpriteRenderer sprender;
    private float nextFire = 0.0F;
    Object bulletRef;

    private Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        sprender = GetComponent<SpriteRenderer>();
        poly = GetComponent<PolygonCollider2D>();
        bulletRef = Resources.Load("Bullet");
        AudioSource[] audioSources = GetComponents<AudioSource>();
        Thrusters = audioSources[0];
        ShipExplosion = audioSources[1];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > nextFire && sprender.enabled != false)
        {
            nextFire = Time.time + fireRate;
            GameObject bullet = (GameObject)Instantiate(bulletRef, transform.position, transform.rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = rb2d.velocity + (Vector2)(transform.up * bulletSpeed);
            bullet.GetComponent<Rigidbody2D>().velocity = Vector2.ClampMagnitude(bullet.GetComponent<Rigidbody2D>().velocity, maxBulletSpeed);
        }
    }

    // FixedUpdate is called once per fixed physics frame
    void FixedUpdate()
    {
        xPos = transform.position.x;
        yPos = transform.position.y;
        //Handle wrapping offscreen
        if (Mathf.Abs(xPos) > 195)
        {
            transform.position = new Vector2(Mathf.Sign(xPos) * 195 * -1, yPos);
        }
        if (Mathf.Abs(yPos) > 118)
        {
            transform.position = new Vector2(xPos, Mathf.Sign(yPos) * 118 * -1);
        }

        if(Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        //Handle Thrust
        if (Input.GetKey(KeyCode.I))
        {
            rb2d.AddForce(transform.up * speed);
            rb2d.velocity = Vector2.ClampMagnitude(rb2d.velocity, maxSpeed);
            if(!Thrusters.isPlaying)
                Thrusters.Play();
        }
        else
        {
            if(Thrusters.isPlaying)
                Thrusters.Stop();
        }

        //Handle Turning
        if(Input.GetKey(KeyCode.J) || Input.GetKey(KeyCode.L))
        {
            if(Input.GetKey(KeyCode.J))
                rb2d.angularVelocity = 10 * turnSpeed;
            if(Input.GetKey(KeyCode.L))
                rb2d.angularVelocity = -10 * turnSpeed;
        }
        else
        {
            rb2d.angularVelocity = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Ship plays its sound and destroys itself
        ShipExplosion.Play();
        GameObject explosion = Instantiate(explosionPrefab);
        explosion.transform.position = transform.position;
        AsteroidExplosion = collision.gameObject.GetComponent<AudioSource>();
        AsteroidExplosion.Play();
        sprender.enabled = false;
        poly.enabled = false;
        Thrusters.enabled = false;
        GameController.timeDied = Time.time;
        Destroy(gameObject, 4f);
    }
}