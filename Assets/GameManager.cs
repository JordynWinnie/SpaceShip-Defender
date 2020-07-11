﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    public static int HighScore = 0;
    public int Lives { get; set; }
    public int Score { get; set; }
    public int Coins { get; set; }
    public bool doubleScore { get; set; }
    public bool isPaused { get; set; }
    public bool isTimeFrozen { get; set; }

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
    private OnDeathAnimation deathAnimationManager;

    private void Awake()
    {
        Lives = 100;
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
        deathAnimationManager = FindObjectOfType<OnDeathAnimation>();
        spawnManager = FindObjectOfType<SpawningManagement>();
        currentGameState = GameState.InGame;
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
                    SetPauseState(true);

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
                    SetPauseState(false);
                    currentGameState = GameState.InGame;
                    pauseMenu.SetActive(false);
                    Time.timeScale = 1;
                }
                break;

            case GameState.GameOver:
                if (Input.GetKeyDown(KeyCode.R))
                {
                    currentGameState = GameState.InGame;
                    Time.timeScale = 1;
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    currentGameState = GameState.InGame;
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
        SetPauseState(true);
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
        isTimeFrozen = isEnabled;
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

    public void SetPauseState(bool pauseState)
    {
        isPaused = pauseState;
    }

    public void PlayExplosionAnimation(Transform explosionLocation, bool isBigExplosion)
    {
        deathAnimationManager.MakeExplosion(explosionLocation, isBigExplosion);
    }
}