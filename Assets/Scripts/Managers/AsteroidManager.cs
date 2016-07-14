using UnityEngine;
using System.Collections;

public class AsteroidManager : MonoBehaviour {

    int level;
    // Asteroid Variables
    public int m_minAsteroids = 3;
    public int m_maxAsteroids = 10;        
    public int m_maxTries = 50;
    public float m_asteroidLevelRatio = 10000f;
    public GameObject m_asteroidPrefab;
    bool gameOver;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (gameOver)
        {
            foreach (GameObject asteroid in GameObject.FindGameObjectsWithTag("Asteroid"))
                asteroid.GetComponent<AsteroidController>().Die(0f);
        }
	}


    public void NewGame()
    {
        gameOver = false;
    }

    public void NewLevel(int newLevel)
    {
        level = newLevel;
        int asteroids = m_minAsteroids + Mathf.RoundToInt(level / m_asteroidLevelRatio);
        asteroids = asteroids > m_maxAsteroids ? m_maxAsteroids : asteroids;

        GameObject[] m_spawns = GameObject.FindGameObjectsWithTag("AsteroidSpawn");

        while (asteroids > 0)
        {
            int spawn = Random.Range(0, m_spawns.Length - 1);
            print("Spawning asteroid at" + spawn);
            Vector3 newPosition = m_spawns[spawn].transform.position;
            Instantiate(m_asteroidPrefab, newPosition, Random.rotation);
            asteroids--;
        }
    }

    public void GameOver()
    {
        gameOver = true;
    }
}
