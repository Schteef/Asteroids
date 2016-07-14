using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

    public int score;
    public int m_extraLifeCost;
    public int nextExtraLife;

    public int lives = 3;
    public float m_playerSpawnDelay = 3f;
    float nextPlayerSpawn;

    public GameObject m_playerPrefab;
    public Transform m_playerSpawnPoint;
    public AudioSource m_extraLifeSound;

    bool gameOver = false;
    PlayerController player;
    int level;

    

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void NewGame()
    {
        gameOver = false;
        lives = 3;
        score = 0;
        nextExtraLife = m_extraLifeCost;
    }

    public void NewLevel(int newLevel)
    {
        level = newLevel;
    }

    public bool SpawnPlayer()
    {

        if (lives > 0)
        {
            lives--;
        }
        else
        {            
            return false;
        }
        GameObject newPlayer = Instantiate(m_playerPrefab, m_playerSpawnPoint.position, m_playerSpawnPoint.rotation) as GameObject;
        player = newPlayer.GetComponent<PlayerController>();
        return true;
    }

    public void AwardPoints(int points)
    {
        if (!gameOver)
        {
            score += points;
            if (score > nextExtraLife)
            {
                nextExtraLife += m_extraLifeCost;
                m_extraLifeSound.Play();
                lives++;
            }
        }
    }

    public bool PlayerAlive()
    {
        if (player.m_IsDead)
        {
            nextPlayerSpawn = player.m_TimeOfDeath + m_playerSpawnDelay;
            if (nextPlayerSpawn < Time.time)
            {
                Destroy(player.gameObject);
                return SpawnPlayer();
            }
        }

        return true;
    }

    public void GameOver()
    {
        gameOver = true;
    }

}
