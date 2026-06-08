using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event EventHandler OnGameOver;
    public event EventHandler OnScoreChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;

    public enum GameState
    {
        Gameplaying,
        Paused,
        GameOver
    }

    public GameState CurrentGameState => currentGameState;
    public int CurrentScore => Mathf.FloorToInt(currentScore);
    public int BestScore => PlayerPrefs.GetInt("BestScore", 0);
    public float CurrentScoreMultiplier => currentScoreMultiplier;
    public float WorldSpeed => worldSpeed;

    [Header("Game State")]
    [SerializeField] private GameState startingGameState = GameState.Gameplaying;

    [Header("Score")]
    [SerializeField] private float baseScorePerSecond = 10f;
    [SerializeField] private float multiplierIncreasePerSecond = 0.05f;
    [SerializeField] private float maxMultiplier = 10f;

    [Header("World Speed")]
    [SerializeField] private float startWorldSpeed = 10f;
    [SerializeField] private float speedIncreasePerScore = 0.03f;
    [SerializeField] private float maxWorldSpeed = 30f;

    private GameState currentGameState;

    private float currentScore;
    private float currentScoreMultiplier = 1f;
    private float worldSpeed;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        worldSpeed = startWorldSpeed;
        SetGameState(startingGameState);
    }

    private void Update()
    {
        if (currentGameState != GameState.Gameplaying)
            return;

        UpdateScore();
    }

    private void UpdateScore()
    {
        currentScoreMultiplier += multiplierIncreasePerSecond * Time.deltaTime;
        currentScoreMultiplier = Mathf.Min(currentScoreMultiplier, maxMultiplier);

        currentScore += baseScorePerSecond * currentScoreMultiplier * Time.deltaTime;

        worldSpeed = Mathf.Min(
            startWorldSpeed + CurrentScore * speedIncreasePerScore,
            maxWorldSpeed
        );

        OnScoreChanged?.Invoke(this, EventArgs.Empty);
    }

    public void ResetScore()
    {
        currentScore = 0f;
        currentScoreMultiplier = 1f;
        worldSpeed = startWorldSpeed;

        OnScoreChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetGameState(GameState newGameState)
    {
        if (currentGameState == newGameState)
            return;

        currentGameState = newGameState;

        switch (currentGameState)
        {
            case GameState.Gameplaying:
                Time.timeScale = 1f;
                ResetScore();
                OnGameUnpaused?.Invoke(this, EventArgs.Empty);
                break;

            case GameState.Paused:
                Time.timeScale = 0f;
                OnGamePaused?.Invoke(this, EventArgs.Empty);
                break;

            case GameState.GameOver:
                Time.timeScale = 0f;

                int finalScore = CurrentScore;
                int best = BestScore;

                if (finalScore > best)
                {
                    PlayerPrefs.SetInt("BestScore", finalScore);
                    PlayerPrefs.Save();
                }

                OnGameOver?.Invoke(this, EventArgs.Empty);
                break;
        }
    }

    public void TogglePause()
    {
        if (currentGameState == GameState.Gameplaying)
            SetGameState(GameState.Paused);
        else if (currentGameState == GameState.Paused)
            SetGameState(GameState.Gameplaying);
    }

    public void EndGame()
    {
        SetGameState(GameState.GameOver);
    }
}