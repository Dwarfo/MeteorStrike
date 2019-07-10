using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : Singleton_MB<UI> {

    public Button resetGameButton;
    public Button testButton;

    public Text scoreText;
    public Text collisionCheckNum;
    public GameObject Graph;

    private int score = 0;

	void Start ()
    {
        resetGameButton.onClick.AddListener(HandleResetButton);
        testButton.onClick.AddListener(HandleTestButton);
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void UpdateCollisionStatistics(int num)
    {
        collisionCheckNum.text = num.ToString();
    }

    private void HandleTestButton()
    {
        GameManager.Instance.MakeTestMeteor();
    }

    private void HandleResetButton()
    {
        GameManager.Instance.RestartGame();
    }

    private void HandlePlayerDeath()
    {
        resetGameButton.gameObject.SetActive(true);
    }

    public void AddScore()
    {
        score++;
        scoreText.text = score.ToString();
    }
}
