using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour {

    public GameObject m_enemyPrefab;
    public float m_enemySpawnFrequency = 30f;
    public float m_enemySpawnRange = 30f;
    public float m_smartEnemyThreshold = 10;
    public float nextEnemy;
    public Transform m_enemySpawnPoint;

    bool gameOver;
    int level;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
    void Update () {
        if (gameOver)
        {
            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("EnemyShip"))
                enemy.GetComponent<EnemyController>().Die();
        }
    }


    public void CheckEnemySpawn()
    {
        if (nextEnemy == 0)
        {
            nextEnemy = Time.time + m_enemySpawnFrequency + Random.Range(0, m_enemySpawnRange);
            return;
        }
        if (Time.time > nextEnemy)
        {
            if (GameObject.FindGameObjectWithTag("EnemyShip"))
            {
                nextEnemy = Time.time + m_enemySpawnFrequency;
            }
            else
            {
                bool isSmart = false;

                if (level > m_smartEnemyThreshold)
                    isSmart = true;
                else
                    isSmart = Random.Range(0, m_smartEnemyThreshold) < level;


                // Spawn an Enemy
                GameObject enemy = Instantiate(m_enemyPrefab, new Vector3(0, -20, 0), m_enemySpawnPoint.transform.rotation) as GameObject;
                EnemyController enemyController = enemy.GetComponent<EnemyController>();
                if (isSmart)
                    enemyController.SmartenUp();
            }
        }
    }

    public void NewGame()
    {
        nextEnemy = 0;
        gameOver = false;

    }

    public void NewLevel(int newLevel)
    {
        level = newLevel;
    }

    public void GameOver()
    {
        gameOver = true;
    }
}
