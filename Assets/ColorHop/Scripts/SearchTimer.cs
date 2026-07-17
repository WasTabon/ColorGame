using System;
using UnityEngine;

public class SearchTimer : MonoBehaviour
{
    public float InitialMaxTime = 3f;
    public float MinMaxTime = 1.2f;
    public float DecreasePerBlock = 0.05f;

    public float CurrentTime { get; private set; }
    public float CurrentMaxTime { get; private set; }
    public bool IsRunning { get; private set; }

    public event Action<float, float> OnTimeChanged;
    public event Action OnTimeout;

    private int blocksBroken;

    public void StartTimer()
    {
        blocksBroken = 0;
        CurrentMaxTime = InitialMaxTime;
        CurrentTime = CurrentMaxTime;
        IsRunning = true;
        if (OnTimeChanged != null) OnTimeChanged(CurrentTime, CurrentMaxTime);
    }

    public void StopTimer()
    {
        IsRunning = false;
    }

    public void ResetOnMatch()
    {
        blocksBroken++;
        float newMax = Mathf.Max(MinMaxTime, InitialMaxTime - blocksBroken * DecreasePerBlock);
        CurrentMaxTime = newMax;
        CurrentTime = CurrentMaxTime;
        if (OnTimeChanged != null) OnTimeChanged(CurrentTime, CurrentMaxTime);
    }

    private void Update()
    {
        if (!IsRunning) return;

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
