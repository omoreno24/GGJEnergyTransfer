using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {
    public GameObject GameOverUI;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	public void OnGameOver () {
        GameOverUI.SetActive(true);

    }

    public void RestartScene()
    {
        SceneManager.LoadScene(0);
    }
}
