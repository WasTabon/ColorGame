using UnityEngine;

public class RowContainer : MonoBehaviour
{
    public BlockCell[] Cells;
    public RectTransform Rect;

    public void ApplyColors(int[] colorIndices)
    {
        Debug.Assert(colorIndices.Length == Cells.Length, "colorIndices length mismatch");
        for (int i = 0; i < Cells.Length; i++)
        {
            Cells[i].SetColor(colorIndices[i]);
        }
    }

    public int GetColorAt(int column)
    {
        return Cells[column].ColorIndex;
    }

    public bool ContainsColor(int colorIndex)
    {
        for (int i = 0; i < Cells.Length; i++)
        {
            if (Cells[i].ColorIndex == colorIndex) return true;
        }
        return false;
    }
}
