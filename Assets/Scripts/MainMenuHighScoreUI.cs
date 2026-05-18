using TMPro;
using UnityEngine;

public class MainMenuHighScoreUI : MonoBehaviour {

    public static MainMenuHighScoreUI Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI highScoreTextMesh;


    private void Awake() {
        Instance = this;
    }

    private void Start() {
        Show();
        UpdateHighScore();
    }

    private void UpdateHighScore() {

        highScoreTextMesh.text = $"{HighScoreManager.GetHighScoreClassic()}";


    }

    public void Show() {
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }
}