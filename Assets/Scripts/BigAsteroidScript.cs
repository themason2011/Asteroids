using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigAsteroidScript : MonoBehaviour
{
    public AudioSource AsteroidExplosion;
    public GameObject explosionPrefab;

    private float xPos;
    private float yPos;
    private PolygonCollider2D poly;
    private SpriteRenderer sprender;
    private Rigidbody2D rb2d;
    Object smallAsteroidRef;
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.velocity = new Vector2(Random.Range(-20.0f, 20.0f), Random.Range(-20.0f, 20.0f));
        smallAsteroidRef = Resources.Load("SmallAsteroid");
        AsteroidExplosion = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet" || collision.gameObject.tag == "Spaceship")
        {
            //Asteroid plays its song and destroys itself
            AsteroidExplosion.Play();
            //Add Score
            ScoreScript.scoreValue += 1000;
            //Hide object and disable collision
            sprender = gameObject.GetComponent<SpriteRenderer>();
            sprender.enabled = false;
            poly = gameObject.GetComponent<PolygonCollider2D>();
            poly.enabled = false;
            //Create 2 new, smaller asteroids with same general direction as bigger asteroid and some variance included
            GameObject smallAsteroid1 = (GameObject)Instantiate(smallAsteroidRef, new Vector3(transform.position.x - 7f, transform.position.y - 7f, 0), Quaternion.Euler(0, 0, 90));
            smallAsteroid1.GetComponent<Rigidbody2D>().velocity = (rb2d.velocity * 1.5f) + new Vector2(Random.Range(-20.0f, 20.0f), Random.Range(-20.0f, 20.0f));
            GameObject smallAsteroid2 = (GameObject)Instantiate(smallAsteroidRef, new Vector3(transform.position.x + 7f, transform.position.y + 7f, 0), Quaternion.Euler(0, 0, 0));
            smallAsteroid2.GetComponent<Rigidbody2D>().velocity = (rb2d.velocity * 1.5f) + new Vector2(Random.Range(-20.0f, 20.0f), Random.Range(-20.0f, 20.0f));
            GameObject explosion = Instantiate(explosionPrefab);
            explosion.transform.position = transform.position;

            gameObject.tag = "Untagged";
            //Wait 4 seconds to actually destroy object
            Destroy(gameObject, 4f);
        }
    }
}
