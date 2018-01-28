using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    private int _score;
    public int Score { 
        get{
            return _score; 
        }
        set{
            _score = value;
            UpdateScoreBoard();
        }

    }
    public Text ScoreText;
    public PlayerController player1;
    public PlayerController player2;
    public GameObject GameplayUI;
    public GameObject GameOverUI;
    public Image player1HealthBar;
    public Image player2HealthBar;
	
	
	// Update is called once per frame
	void Update () {
        if(GameplayUI.activeSelf)
        {
            if (player1!= null && player1HealthBar!= null)
            {
                player1HealthBar.fillAmount = player1.properties.HealthPoints / 100;
            }

            if(player2!= null && player2HealthBar!= null)
            {
                player2HealthBar.fillAmount = player2.properties.HealthPoints / 100;
            }
        }
	}
    public void UpdateScoreBoard(){
        ScoreText.text = Score.ToString();
    }
    public void OnGameOver()
    {
        GameOverUI.SetActive(true);

    }
    public void RestartScene()
    {
        SceneManager.LoadScene(0);
    }
}
