using UnityEngine;

public class StatsManager : MonoBehaviour
{
    private const string PREF_BEST_SCORE = "best_score";
    private const string PREF_TOTAL_GAMES = "total_games";
    private const string PREF_TOTAL_BLOCKS = "total_blocks_broken";
    private const string PREF_BEST_COMBO = "best_combo";

    public static int GetBestScore()
    {
        return PlayerPrefs.GetInt(PREF_BEST_SCORE, 0);
    }

    public static int GetTotalGames()
    {
        return PlayerPrefs.GetInt(PREF_TOTAL_GAMES, 0);
    }

    public static int GetTotalBlocksBroken()
    {
        return PlayerPrefs.GetInt(PREF_TOTAL_BLOCKS, 0);
    }

    public static int GetBestCombo()
    {
        return PlayerPrefs.GetInt(PREF_BEST_COMBO, 0);
    }

    public static void RegisterGamePlayed()
    {
        int games = GetTotalGames() + 1;
        PlayerPrefs.SetInt(PREF_TOTAL_GAMES, games);
        PlayerPrefs.Save();
    }

    public static void RegisterBlockBroken()
    {
        int blocks = GetTotalBlocksBroken() + 1;
        PlayerPrefs.SetInt(PREF_TOTAL_BLOCKS, blocks);
        PlayerPrefs.Save();
    }

    public static void RegisterComboIfBest(int combo)
    {
        if (combo > GetBestCombo())
        {
            PlayerPrefs.SetInt(PREF_BEST_COMBO, combo);
            PlayerPrefs.Save();
        }
    }
}
