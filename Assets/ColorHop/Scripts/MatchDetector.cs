using System;
using UnityEngine;

public class MatchDetector : MonoBehaviour
{
    public GridManager Grid;
    public PlayerCube Player;

    public float RequiredHoldTime = 0.1f;

    public event Action<RowContainer, int> OnMatch;

    private bool active;
    private float holdTime;
    private RowContainer lockedRow;
    private int lockedColumn = -1;

    public void StartDetection()
    {
        active = true;
        holdTime = 0f;
        lockedRow = null;
        lockedColumn = -1;
    }

    public void StopDetection()
    {
        active = false;
    }

    private void Update()
    {
        if (!active) return;

        RowContainer row = Grid.FindTopRow();
        if (row == null)
        {
            holdTime = 0f;
            lockedRow = null;
            lockedColumn = -1;
            return;
        }

        if (!row.ContainsColor(Player.ColorIndex))
        {
            Grid.EnsureColorInTopRow(Player.ColorIndex);
        }

        int col = Grid.GetColumnAtX(Player.Rect.anchoredPosition.x);
        int rowColorAtCol = row.GetColorAt(col);

        if (rowColorAtCol != Player.ColorIndex)
        {
            holdTime = 0f;
            lockedRow = null;
            lockedColumn = -1;
            return;
        }

        if (lockedRow != row || lockedColumn != col)
        {
            lockedRow = row;
            lockedColumn = col;
            holdTime = 0f;
        }

        holdTime += Time.deltaTime;
        if (holdTime >= RequiredHoldTime)
        {
            RowContainer matched = lockedRow;
            int matchedCol = lockedColumn;
            holdTime = 0f;
            lockedRow = null;
            lockedColumn = -1;
            if (OnMatch != null) OnMatch(matched, matchedCol);
        }
    }
}
