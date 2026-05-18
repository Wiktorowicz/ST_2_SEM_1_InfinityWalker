using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Buttons")]
    public Button playButton;
    public Button settingsButton;
    public Button aboutButton;
    public Button quitButton;

    [Header("Panels")]
    public GameObject settingsPanel;
    public GameObject aboutPanel;

    [Header("Scene")]
    public string playScene;

    void Start()
    {
        if (playButton != null)
            playButton.onClick.AddListener(Play);

        if (settingsButton != null)
            settingsButton.onClick.AddListener(OpenSettings);

        if (aboutButton != null)
            aboutButton.onClick.AddListener(OpenAbout);

        if (quitButton != null)
            quitButton.onClick.AddListener(Quit);

        // Na start wyłącz wszystko (pewność że nic się nie nałoży)
        ClosePanels();
    }

    void Play()
    {
        SoundManager.Instance.PlayButtonClick(Vector3.zero);

        SceneManager.LoadScene(playScene);
    }

    void OpenSettings()
    {

        SoundManager.Instance.PlayButtonClick(Vector3.zero);

        // jeśli już otwarty → zamknij
        if (settingsPanel.activeSelf)
        {
            settingsPanel.SetActive(false);
            return;
        }

        // w innym przypadku → zamknij inne i otwórz ten
        ClosePanels();
        settingsPanel.SetActive(true);
    }

    void OpenAbout()
    {

        SoundManager.Instance.PlayButtonClick(Vector3.zero);

        if (aboutPanel.activeSelf)
        {
            aboutPanel.SetActive(false);
            return;
        }

        ClosePanels();
        aboutPanel.SetActive(true);
    }

    public void ClosePanels()
    {

        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        if (aboutPanel != null)
            aboutPanel.SetActive(false);
    }

    void Quit()
    {
        SoundManager.Instance.PlayButtonClick(Vector3.zero);


        Application.Quit();
    }
}