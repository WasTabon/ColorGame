using UnityEngine;
using UnityEngine.UI;

public class BlockCell : MonoBehaviour
{
    public Image Image;
    private int colorIndex;

    public int ColorIndex { get { return colorIndex; } }

    public void SetColor(int index)
    {
        colorIndex = index;
        Image.color = ColorPalette.Colors[index];
    }
}
