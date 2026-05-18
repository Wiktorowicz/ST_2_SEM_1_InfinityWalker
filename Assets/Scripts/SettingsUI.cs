using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour {

    public static SettingsUI Instance { get; private set; }

    [SerializeField] private Button closeButton;
    [SerializeField] private Slider soundEffectsSlider;
    [SerializeField] private TextMeshProUGUI soundEffectsText;


    private void Awake() {
        Instance = this;


        soundEffectsSlider.onValueChanged.AddListener((float value) => {
            SoundManager.Instance.SetVolume(Mathf.RoundToInt(value));
            UpdateVisual();
        });

        closeButton.onClick.AddListener(() => {
            SoundManager.Instance.PlayButtonClick(Vector3.zero);

            Hide();
        });
    }

    private void Start() {
        soundEffectsSlider.value = SoundManager.Instance.GetVolume() * 100f;
        UpdateVisual();
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


    public void Show() {
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }


}