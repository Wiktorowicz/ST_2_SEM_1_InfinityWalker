using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button quitButton;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI bestScoreText;

    [Header("Scene")]
    [SerializeField] private string menuSceneName;

    private void Awake()
    {
        if (resumeButton != null)
        {
            resumeButton.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlayButtonClick(Vector3.zero);

                GameManager.Instance.SetGameState(GameManager.GameState.Gameplaying);
                Hide();
            });
        }

        if (restartButton != null)
        {

            restartButton.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlayButtonClick(Vector3.zero);
                GameManager.Instance.SetGameState(GameManager.GameState.Gameplaying);
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            });
        }

        if (quitButton != null)
        {

            quitButton.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlayButtonClick(Vector3.zero);

                if (!string.IsNullOrEmpty(menuSceneName))
                    SceneManager.LoadScene(menuSceneName);
                else
                    Debug.LogWarning("Menu scene name not set!");
            });
        }
    }

    private void Start()
    {
        GameManager.Instance.OnGameOver += GameManager_OnGameOver;
        Hide();
    }

    private void GameManager_OnGameOver(object sender, System.EventArgs e)
    {
        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0f;

        int score = GameManager.Instance.CurrentScore;
        int best = GameManager.Instance.BestScore;

        bestScoreText.text = "BEST SCORE: " + best;
        scoreText.text = "CURRENT SCORE: " + score;
    }

    private void Hide()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}