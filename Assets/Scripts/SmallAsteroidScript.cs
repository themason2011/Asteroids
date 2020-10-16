using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallAsteroidScript : MonoBehaviour
{
    public AudioSource AsteroidExplosion;
    public GameObject explosionPrefab;

    private float xPos;
    private float yPos;
    private PolygonCollider2D poly;
    private SpriteRenderer sprender;
    private Rigidbody2D rb2d;
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
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
            ScoreScript.scoreValue += 100;
            //Hide object and disable collision
            sprender = gameObject.GetComponent<SpriteRenderer>();
            sprender.enabled = false;
            poly = gameObject.GetComponent<PolygonCollider2D>();
            poly.enabled = false;
            GameObject explosion = Instantiate(explosionPrefab);
            explosion.transform.position = transform.position;

            //Check if game has been won
            gameObject.tag = "Untagged";
            GameObject[] asteroids = GameObject.FindGameObjectsWithTag("BigAsteroid");
            if (asteroids.Length == 0)
            {
                GameObject[] asteroids2 = GameObject.FindGameObjectsWithTag("SmallAsteroid");
                if (asteroids2.Length == 0)
                {
                    GameController.levelEnd = true;
                }
            }

            //Wait 4 seconds to actually destroy object
            Destroy(gameObject, 4f);
        }
    }
}
