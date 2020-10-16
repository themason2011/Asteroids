using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public AudioSource AsteroidExplosion;
    public float shotTime = 1.2f;

    private float xPos;
    private float yPos;
    private float startTime;
    private BoxCollider2D box;
    private SpriteRenderer sprender;
    private Rigidbody2D rb2d;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        rb2d = GetComponent<Rigidbody2D>();
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

        //Handle time alive
        if (Time.time - startTime >= shotTime)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Bullet destroys itself
        Destroy(gameObject);
    }
}