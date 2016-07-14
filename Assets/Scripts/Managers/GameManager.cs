using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    
    public GameObject m_asteroidPrefab;


    //Sound Variables
    public AudioSource m_newLevelSound;    
    public AudioSource m_gameStartSound;

    // Player Variables
    PlayerManager m_player;
    EnemyManager m_enemy;
    AsteroidManager m_asteroid;

    int level;
    
    
    public Scoreboard scoreboard;    
    
    bool gameOver;

    

	// Use this for initialization
	void Start () {
        // Do Pregame setup
        


        // Start Gameloop Coroutine
        StartCoroutine(GameLoop());
    }

    IEnumerator GameLoop()
    {
        while (true)
        {
            yield return StartCoroutine(ShowTitleScreen());
            yield return StartCoroutine(PlayGame());
            yield return StartCoroutine(ShowEndScreen());
        }

    }

    IEnumerator ShowTitleScreen()
    {        
        // End the Titlescreen Sequence.
        scoreboard.ShowGameStartScreen();
        
        while (!Input.GetButton("Fire1"))
            yield return null;
        m_gameStartSound.Play();
        yield return new WaitForSeconds(1);
        
    }

    IEnumerator PlayGame()
    {
        StartNewGame();
        m_player.SpawnPlayer();
        StartNewLevel();
        while (m_player.PlayerAlive())
        {
            if (LevelFinished())
                StartNewLevel();

            m_enemy.CheckEnemySpawn();
            
            scoreboard.UpdateScoreboard(m_player.score, m_player.lives, level);
            yield return null;
        }

        m_player.GameOver();
        m_enemy.GameOver();
        m_asteroid.GameOver();
        yield return null;
    }

    IEnumerator ShowEndScreen()
    {
        gameOver = true;
        scoreboard.ShowGameOverScreen();
      
            while (!Input.GetButton("Fire1"))
            if (Input.GetKey("escape"))
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            else
                yield return null;

        Bounds boundary = GameObject.FindWithTag("Boundary").GetComponent<TeleportBoundary>().boundary;
        Collider[] objects = Physics.OverlapBox(boundary.center, boundary.extents);
        foreach (Collider collider in objects)
        {
            Destroy(collider.gameObject);
            yield return null;
        }

        yield return new WaitForSeconds(1);
    }



    public void StartNewGame()
    {
        level = 0;
        m_player = GetComponent<PlayerManager>();
        m_player.NewGame();

        m_enemy = GetComponent<EnemyManager>();
        m_enemy.NewGame();

        m_asteroid = GetComponent<AsteroidManager>();
        m_asteroid.NewGame();
            
        gameOver = false;
        scoreboard.ShowScoreScreen();

    }

    public void StartNewLevel()
    {
        level++;
        m_player.NewLevel(level);
        m_asteroid.NewLevel(level);
        m_enemy.NewLevel(level);
        
    }

    public bool LevelFinished()
    {
        GameObject[] asteroids = GameObject.FindGameObjectsWithTag("Asteroid");
        return asteroids.Length == 0;        
    }         
    
}
