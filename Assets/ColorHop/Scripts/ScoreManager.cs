using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private const string PREF_BEST = "best_score";

    public int Score { get; private set; }
    public int BestScore { get; private set; }

    public event Action<int> OnScoreChanged;
    public event Action<int> OnBestScoreChanged;

    private void Awake()
    {
        BestScore = PlayerPrefs.GetInt(PREF_BEST, 0);
    }

    public void ResetScore()
    {
        Score = 0;
        if (OnScoreChanged != null) OnScoreChanged(Score);
    }

    public void AddScore(int amount)
    {
        Score += amount;
        if (OnScoreChanged != null) OnScoreChanged(Score);

        if (Score > BestScore)
        {
            BestScore = Score;
            PlayerPrefs.SetInt(PREF_BEST, BestScore);
            PlayerPrefs.Save();
            if (OnBestScoreChanged != null) OnBestScoreChanged(BestScore);
        }
    }
}
