using UnityEngine;

public static class HighScoreManager {


    private const string PLAYER_PREFS_HIGH_SCORE = "BestScore";



    public static int GetHighScoreClassic() {
        return PlayerPrefs.GetInt(PLAYER_PREFS_HIGH_SCORE, 0);
    }



}
