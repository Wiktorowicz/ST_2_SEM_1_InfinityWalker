using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class PauseManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject pausePanel;

    [Header("Buttons")]
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button quitButton;

    [Header("Scenes")]
    [SerializeField] private string gameSceneName;
    [SerializeField] private string menuSceneName;

    [Header("Audio")]
    [SerializeField] private Slider soundEffectsSlider;
    [SerializeField] private TextMeshProUGUI soundEffectsText;

    private bool isPaused = false;

    private void Start()
    {
        Time.timeScale = 1f;
        isPaused = false;


        soundEffectsSlider.value = SoundManager.Instance.GetVolume() * 100f;

        soundEffectsSlider.onValueChanged.AddListener((float value) => {
            SoundManager.Instance.SetVolume(Mathf.RoundToInt(value));
            UpdateVisual();
        });

        UpdateVisual();


        if (pausePanel != null)
            pausePanel.SetActive(false);

        if (resumeButton != null)
            resumeButton.onClick.AddListener(Resume);

        if (restartButton != null)
            restartButton.onClick.AddListener(Restart);

        if (quitButton != null)
            quitButton.onClick.AddListener(QuitToMenu);
    }

    private void Update() {
        if (GameManager.Instance.CurrentGameState != GameManager.GameState.Gameplaying)
            return;

        if (Keyboard.current.escapeKey.wasPressedThisFrame) {
            TogglePause();
        }
    }

    private void TogglePause()
    {
        if (isPaused)
            Resume();
        else
            Pause();
    }

    private void Pause()
    {
        SoundManager.Instance.PlayButtonClick(Vector3.zero);

        Time.timeScale = 0f;
        isPaused = true;

        if (pausePanel != null)
            pausePanel.SetActive(true);
    }

    private void Resume()
    {
        SoundManager.Instance.PlayButtonClick(Vector3.zero);

        Time.timeScale = 1f;
        isPaused = false;

        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    private void Restart()
    {
        SoundManager.Instance.PlayButtonClick(Vector3.zero);

        Time.timeScale = 1f;
        SceneManager.LoadScene(gameSceneName);
    }

    private void QuitToMenu()
    {
        SoundManager.Instance.PlayButtonClick(Vector3.zero);

        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneName);
    }


    private void UpdateVisual() {
        int volume = Mathf.RoundToInt(soundEffectsSlider.value);

        if (volume == 0) {
            soundEffectsText.text = "SFX Volume: OFF";
        }
        else {
            soundEffectsText.text = "SFX Volume: " + volume + "%";
        }
    }
}