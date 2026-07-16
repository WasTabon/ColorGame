using UnityEngine;

public static class ColorPalette
{
    public static readonly Color[] Colors = new Color[]
    {
        new Color(0.29f, 0.56f, 0.89f, 1f),
        new Color(0.96f, 0.26f, 0.51f, 1f),
        new Color(0.96f, 0.65f, 0.14f, 1f),
        new Color(0.4f,  0.78f, 0.31f, 1f),
        new Color(0.64f, 0.21f, 0.93f, 1f)
    };

    public static readonly Color Background = new Color(0.1f, 0.1f, 0.18f, 1f);
    public static readonly Color Primary    = new Color(0.29f, 0.56f, 0.89f, 1f);
    public static readonly Color Accent     = new Color(0.96f, 0.65f, 0.14f, 1f);

    public static int GetRandomIndex()
    {
        return Random.Range(0, Colors.Length);
    }

    public static int GetRandomIndexExcept(int except)
    {
        if (Colors.Length <= 1) return 0;
        int idx;
        do { idx = Random.Range(0, Colors.Length); } while (idx == except);
        return idx;
    }
}
