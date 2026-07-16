using System;
using UnityEngine;

public class SearchTimer : MonoBehaviour
{
    public float InitialMaxTime = 3f;
    public float MinMaxTime = 1.2f;
    public float DecreaseInterval = 15f;
    public float DecreaseAmount = 0.15f;

    public float CurrentTime { get; private set; }
    public float CurrentMaxTime { get; private set; }
    public bool IsRunning { get; private set; }

    public event Action<float, float> OnTimeChanged;
    public event Action OnTimeout;

    private float gameElapsed;

    public void StartTimer()
    {
        CurrentMaxTime = InitialMaxTime;
        CurrentTime = CurrentMaxTime;
        gameElapsed = 0f;
        IsRunning = true;
        if (OnTimeChanged != null) OnTimeChanged(CurrentTime, CurrentMaxTime);
    }

    public void StopTimer()
    {
        IsRunning = false;
    }

    public void ResetOnMatch()
    {
        CurrentTime = CurrentMaxTime;
        if (OnTimeChanged != null) OnTimeChanged(CurrentTime, CurrentMaxTime);
    }

    private void Update()
    {
        if (!IsRunning) return;

        gameElapsed += Time.deltaTime;
        int decreases = Mathf.FloorToInt(gameElapsed / DecreaseInterval);
        float newMax = Mathf.Max(MinMaxTime, InitialMaxTime - decreases * DecreaseAmount);
        if (Mathf.Abs(newMax - CurrentMaxTime) > 0.001f)
        {
            CurrentMaxTime = newMax;
            if (CurrentTime > CurrentMaxTime) CurrentTime = CurrentMaxTime;
        }

        CurrentTime -= Time.deltaTime;
        if (CurrentTime <= 0f)
        {
            CurrentTime = 0f;
            IsRunning = false;
            if (OnTimeChanged != null) OnTimeChanged(CurrentTime, CurrentMaxTime);
            if (OnTimeout != null) OnTimeout();
            return;
        }

        if (OnTimeChanged != null) OnTimeChanged(CurrentTime, CurrentMaxTime);
    }
}
