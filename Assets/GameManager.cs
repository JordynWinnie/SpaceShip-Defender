﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    public static int HighScore = 0;

    public int Lives { get; set; }
    public int Score { get; set; }
    public int Coins { get; set; }
    public bool doubleScore { get; set; }

    public Text livesText;
    public Text coinsText;
    public Text scoreText;
    public Text scoreBoostText;
    public Text gameOverText;

    public GameObject inGameMenu;
    public GameObject gameOverMenu;
    public GameObject pauseMenu;

    public enum GameState { InGame, Paused, GameOver }

    public GameState currentGameState;
    private SpawningManagement spawnManager;
    private UpgradeManagement upgradeManager;

    private void Awake()
    {
        Lives = 1000;
        Coins = 0;
        Score = 0;

        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        upgradeManager = FindObjectOfType<UpgradeManagement>();
        spawnManager = FindObjectOfType<SpawningManagement>();
        currentGameState = GameState.InGame;
        doubleScore = false;
        coinsText.text = Coins.ToString();
        livesText.text = Lives.ToString();
        scoreText.text = Score.ToString().PadLeft(8, '0');
    }

    private void Update()
    {
        switch (currentGameState)
        {
            case GameState.InGame:
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    currentGameState = GameState.Paused;
                    Time.timeScale = 0;
                    pauseMenu.SetActive(true);
                }

                if (Input.GetKeyDown(KeyCode.P))
                {
                    AddCoins(50);
                }

                if (Input.GetKeyDown(KeyCode.Q))
                {
                    AddScore(100);
                }

                if (Input.GetKeyDown(KeyCode.I))
                {
                    TakeDamage(1);
                }
                break;

            case GameState.Paused:
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    currentGameState = GameState.InGame;
                    pauseMenu.SetActive(false);
                    Time.timeScale = 1;
                }
                break;

            case GameState.GameOver:
                if (Input.GetKeyDown(KeyCode.R))
                {
                    currentGameState = GameState.InGame;
                    gameOverMenu.SetActive(false);
                    Time.timeScale = 1;
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    currentGameState = GameState.InGame;
                    gameOverMenu.SetActive(false);
                    Time.timeScale = 1;
                    SceneManager.LoadScene(0);
                }
                break;
        }
    }

    public void TakeDamage(int damageValue)
    {
        Lives -= damageValue;
        livesText.text = Lives.ToString();

        if (Lives <= 0)
        {
            TriggerLoseCondition();
        }
    }

    private void TriggerLoseCondition()
    {
        if (Score > HighScore)
        {
            HighScore = Score;
        }
        currentGameState = GameState.GameOver;
        gameOverMenu.SetActive(true);
        gameOverText.text = $"Game Over\nScore: {Score.ToString().PadLeft(8, '0')}\nHi-Score: {HighScore.ToString().PadLeft(8, '0')}";
        Time.timeScale = 0;
    }

    public void AddScore(int scoreToAdd)
    {
        Score += scoreToAdd;
        scoreText.text = Score.ToString().PadLeft(8, '0');
    }

    public void AddCoins(int coinsToAdd)
    {
        Coins += coinsToAdd;
        coinsText.text = Coins.ToString();
    }

    public void AddLives(int livesToAdd)
    {
        Lives += livesToAdd;
        livesText.text = Lives.ToString();
    }

    public void ChangeTimeFreeze(bool isEnabled)
    {
        spawnManager.ForceSpawningStop(isEnabled);
    }

    public void ChangeDoubleScore(bool isEnabled)
    {
        doubleScore = isEnabled;
        if (doubleScore)
        {
            scoreBoostText.text = "x2";
        }
        else
        {
            scoreBoostText.text = "";
        }
    }
}