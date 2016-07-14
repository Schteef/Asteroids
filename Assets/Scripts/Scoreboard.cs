using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Scoreboard : MonoBehaviour {

    public GameObject m_gameOverScreen;
    public GameObject m_gameStartScreen;
    public GameObject m_gameOnScreen;

    public Text m_score;
    public Text m_lives;
    public Text m_level;

    public Text m_finalScore;

    public void ShowGameOverScreen()
    {
        m_gameOverScreen.SetActive(true);
        m_gameStartScreen.SetActive(false);
        m_gameOnScreen.SetActive(false);
    }


    public void ShowGameStartScreen()
    {
        m_gameOverScreen.SetActive(false);
        m_gameStartScreen.SetActive(true);
        m_gameOnScreen.SetActive(false);
    }

    public void ShowScoreScreen()
    {
        m_gameOverScreen.SetActive(false);
        m_gameStartScreen.SetActive(false);
        m_gameOnScreen.SetActive(true);
    }

    public void UpdateScoreboard(int score, int lives, int level)
    {
        m_finalScore.text = m_score.text = score.ToString();
        m_lives.text = "Lives : " + lives;
        m_level.text = "Level " + level;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
