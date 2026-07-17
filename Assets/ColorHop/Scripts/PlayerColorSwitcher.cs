using System;
using DG.Tweening;
using UnityEngine;

public class PlayerColorSwitcher : MonoBehaviour
{
    public PlayerCube Player;
    public int MinRowsUntilSwitch = 3;
    public int MaxRowsUntilSwitch = 5;

    public event Action<int> OnColorSwitched;

    private int rowsSinceSwitch;
    private int rowsUntilSwitch;

    public void Initialize()
    {
        rowsSinceSwitch = 0;
        rowsUntilSwitch = UnityEngine.Random.Range(MinRowsUntilSwitch, MaxRowsUntilSwitch + 1);
    }

    public void RegisterRowCleared()
    {
        rowsSinceSwitch++;
        if (rowsSinceSwitch >= rowsUntilSwitch)
        {
            SwitchColor();
            rowsSinceSwitch = 0;
            rowsUntilSwitch = UnityEngine.Random.Range(MinRowsUntilSwitch, MaxRowsUntilSwitch + 1);
        }
    }

    private void SwitchColor()
    {
        int newColor = ColorPalette.GetRandomIndexExcept(Player.ColorIndex);
        Player.PlaySwitchEffect(newColor);
        if (OnColorSwitched != null) OnColorSwitched(newColor);
    }
}
