using UnityEngine;

public class SoundManager : MonoBehaviour {

    private const string PLAYER_PREFS_SOUND_EFFECTS_VOLUME = "SoundEffectsVolume";

    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioClipsSO audioClipsSO;
    [SerializeField] private AudioSource audioSource;

    private int volume;

    private void Awake() {
        Instance = this;
        volume = PlayerPrefs.GetInt(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, 100);
    }

    private void Start() {


    }

    private void PlaySound(AudioClip audioClip, float volumeMultiplier = 1f) {
        audioSource.PlayOneShot(audioClip, volumeMultiplier * GetVolume());
    }

    public void PlayButtonClick(Vector3 position) {
        PlaySound(audioClipsSO.buttonClick);
    }

    public void ChangeVolume() {
        volume += 10;

        if (volume > 100) {
            volume = 0;
        }

        PlayerPrefs.SetInt(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public void SetVolume(int volume) {
        this.volume = volume;

        PlayerPrefs.SetInt(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public float GetVolume() {
        return volume / 100f;
    }
}