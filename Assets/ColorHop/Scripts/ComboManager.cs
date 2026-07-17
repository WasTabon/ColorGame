using System;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    public int GreatThreshold = 3;
    public int AmazingThreshold = 6;
    public int UnstoppableThreshold = 10;

    public int CurrentCombo { get; private set; }

    public event Action<int> OnComboChanged;
    public event Action<string> OnComboMilestone;

    public void ResetCombo()
    {
        CurrentCombo = 0;
        if (OnComboChanged != null) OnComboChanged(CurrentCombo);
    }

    public void RegisterMatch()
    {
        CurrentCombo++;
        if (OnComboChanged != null) OnComboChanged(CurrentCombo);

        string label = GetMilestoneLabel(CurrentCombo);
        if (label != null && OnComboMilestone != null) OnComboMilestone(label);
    }

    public void BreakCombo()
    {
        CurrentCombo = 0;
        if (OnComboChanged != null) OnComboChanged(CurrentCombo);
    }

    private string GetMilestoneLabel(int combo)
    {
        if (combo == GreatThreshold) return "GREAT!";
        if (combo == AmazingThreshold) return "AMAZING!";
        if (combo == UnstoppableThreshold) return "UNSTOPPABLE!";
        if (combo > UnstoppableThreshold && (combo - UnstoppableThreshold) % 5 == 0) return "UNSTOPPABLE!";
        return null;
    }
}
