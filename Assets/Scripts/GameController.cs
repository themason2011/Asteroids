using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private Object asteroidRef;
    private Object spaceshipRef;
    private Object restartRef;
    private GameObject gameOverSign;
    private GameObject winSign;
    private GameObject spaceship;

    public GameObject[] lifeIcons;

    public static int maxLives = 3;
    public static int numLives;
    private float respawnTime = 4f;
    private bool creatingNewLevel = false;

    public static float timeDied;
    public static bool levelEnd = false;

    [SerializeField] private float minCollisionRadius = 10.0f;
    public static int maxAsteroids = 6;
    // Start is called before the first frame update
    void Start()
    {
        numLives = maxLives;
        asteroidRef = Resources.Load("BigAsteroid");
        spaceshipRef = Resources.Load("Spaceship");
        restartRef = Resources.Load("Restart");
        gameOverSign = GameObject.Find("GameOver");
        gameOverSign.SetActive(false);
        winSign = GameObject.Find("Win");
        winSign.SetActive(false);

        startLevel();
        spaceship = (GameObject)Instantiate(spaceshipRef, new Vector2(0, 0), Quaternion.Euler(0, 0, 0));
    }

    private void startLevel()
    {
        for (int i = 0; i < maxAsteroids; i++)
        {
            spawnAsteroid();
        }
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
            
            if(valid && spaceship != null)
                valid = CheckTooCloseToSpaceship(newAsteroid);

        } while (valid == false);
    }

    private bool CheckTooCloseToAsteroid(GameObject testObject)
    {
        GameObject[] asteroids = GameObject.FindGameObjectsWithTag("BigAsteroid");

        foreach(GameObject asteroid in asteroids)
        {
            if(asteroid != testObject)
            {
                if(Vector3.Distance(testObject.transform.position, asteroid.transform.position) < minCollisionRadius)
                {
                    Destroy(testObject);
                    return false;
                }
            }
        }
        return true;
    }

    private bool CheckTooCloseToSpaceship(GameObject testObject)
    {
        if (Vector3.Distance(testObject.transform.position, spaceship.transform.position) < minCollisionRadius)
        {
            Destroy(testObject);
            return false;
        }
        return true;
    }

    IEnumerator NewLevel()
    {
        levelEnd = false;
        creatingNewLevel = true;
        yield return new WaitForSeconds(5.0f);
        Debug.Log("Hi again :)");
        maxAsteroids = maxAsteroids >= 12 ? 12 : maxAsteroids + 2;
        startLevel();
        winSign.SetActive(false);
        creatingNewLevel = false;
    }

    IEnumerator Quit()
    {
        yield return new WaitForSeconds(3.0f);
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        if(levelEnd)
        {
            winSign.SetActive(true);
            StartCoroutine(NewLevel());
        }
        else if (GameObject.FindWithTag("Spaceship") == null && !creatingNewLevel)
        {
            if((Time.time - timeDied) > respawnTime)
            {
                numLives -= 1;
                if(numLives >= 0)
                {
                    Destroy(lifeIcons[numLives]);
                    if (numLives != 0)
                    {
                        spawnSpaceship();
                    }
                }
                else
                {
                    gameOverSign.SetActive(true);
                    StartCoroutine(Quit());
                }
            }
        }
    }

    private void spawnSpaceship()
    {
        bool valid;

        do
        {
            spaceship = (GameObject)Instantiate(spaceshipRef, new Vector2(Random.Range(-175.0f, 175.0f), Random.Range(-100.0f, 100.0f)), Quaternion.Euler(0, 0, 0));

            valid = CheckTooCloseToAsteroid(spaceship);
        } while (valid == false);

        return;
    }
}
