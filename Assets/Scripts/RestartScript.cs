using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartScript : MonoBehaviour
{
    Object asteroidRef;
    Object spaceshipRef;

    private GameObject gameOverSign;
    private GameObject winSign;

    [SerializeField] private float minCollisionRadius = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
        gameOverSign = GameObject.Find("GameOver");
        winSign = GameObject.Find("Win");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseDown()
    {
        asteroidRef = Resources.Load("BigAsteroid");
        spaceshipRef = Resources.Load("Spaceship");

        //Destroy remaining asteroids on restart
        DestroyExistingAsteroids();
        DestroyExistingPlayer();
        ScoreScript.scoreValue = 0;
        gameOverSign.SetActive(false);
        winSign.SetActive(false);
        GameController.numLives = GameController.maxLives;

        //Spawn new asteroids and respawn player
        for (int i = 0; i < GameController.maxAsteroids; i++)
        {
            spawnAsteroid();
        }
        Instantiate(spaceshipRef, new Vector2(0, 0), Quaternion.Euler(0, 0, 0));

        Destroy(gameObject);
    }

    private void spawnAsteroid()
    {
        bool valid;
        GameObject newAsteroid;
        float randX;
        float randY;

        do
        {
            //Ensures the asteroids aren't right next to ship when game starts
            randX = Random.Range(-175.0f, 175.0f);
            randY = Random.Range(-100.0f, 100.0f);
            if (Mathf.Abs(randX) <= 43.0f)
            {
                randX += Mathf.Sign(randX) * 43.0f;
            }
            if (Mathf.Abs(randY) <= 43.0f)
            {
                randX += Mathf.Sign(randX) * 43.0f;
            }

            newAsteroid = (GameObject)Instantiate(asteroidRef, new Vector3(randX, randY, 0), Quaternion.Euler(0, 0, Random.Range(-0.0f, 359.0f)));

            valid = CheckTooCloseToAsteroid(newAsteroid);

        } while (valid == false);
    }

    private bool CheckTooCloseToAsteroid(GameObject testObject)
    {
        GameObject[] asteroids = GameObject.FindGameObjectsWithTag("BigAsteroid");

        foreach (GameObject asteroid in asteroids)
        {
            if (asteroid != testObject)
            {
                if (Vector3.Distance(testObject.transform.position, asteroid.transform.position) < minCollisionRadius)
                {
                    Destroy(testObject);
                    return false;
                }
            }
        }
        return true;
    }

    void DestroyExistingAsteroids()
    {
        GameObject[] asteroids =
            GameObject.FindGameObjectsWithTag("BigAsteroid");

        foreach (GameObject current in asteroids)
        {
            GameObject.Destroy(current);
        }

        GameObject[] asteroids2 =
            GameObject.FindGameObjectsWithTag("SmallAsteroid");

        foreach (GameObject current in asteroids2)
        {
            GameObject.Destroy(current);
        }
    }

    void DestroyExistingPlayer()
    {
        GameObject[] spaceships =
            GameObject.FindGameObjectsWithTag("Spaceship");

        foreach (GameObject current in spaceships)
        {
            GameObject.Destroy(current);
        }
    }
}
