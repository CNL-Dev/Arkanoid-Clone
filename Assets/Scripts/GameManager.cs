using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton

    private static GameManager _instance;

    public static GameManager Instance => _instance;

    private void Awake()
    {
        //Ensures that there is only 1 instance of gameManager
        if(_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    #endregion

    public GameObject gameOverScreen;

    public GameObject victoryScreen;

    public int availableLives = 3;

    public int Lives { get; set; }

    public bool IsGameStarted { get; set; }

    private void Start()
    {
        this.Lives = availableLives;
        Screen.SetResolution(540, 960, false);
        Ball.OnBallDeath += OnBallDeath;
        Brick.OnBrickDestruction += OnBrickDestruction;
    }

    private void OnBrickDestruction(Brick obj)
    {
        if(BricksManager.Instance.RemainingBricks.Count <= 0)
        {
            BallsManager.Instance.ResetBalls();
            GameManager.Instance.IsGameStarted = false;
            BricksManager.Instance.LoadNextLevel();
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnBallDeath(Ball obj)
    {
        if(BallsManager.Instance.Balls.Count <= 0)
        {
            this.Lives--;

            if(this.Lives < 1)
            {
                //Show the gameover screen
                gameOverScreen.SetActive(true);
            }
            else
            {
                //Reset balls
                //Stop the game
                //Reload the level
                BallsManager.Instance.ResetBalls();
                IsGameStarted = false;
                BricksManager.Instance.LoadLevel(BricksManager.Instance.currentLevel);
            }
        }
    }

    private void OnDisable()
    {
        Ball.OnBallDeath -= OnBallDeath;
    }

    internal void ShowVictoryScreen()
    {
        victoryScreen.SetActive(true);
    }
}
