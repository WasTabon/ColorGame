using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    public RectTransform GridRoot;
    public PlayerCube Player;

    public int Columns = 5;
    public float CellSize = 216f;
    public float CellGap = 6f;
    public float ScrollSpeed = 400f;
    public float PlayerY = 620f;
    public int InitialRows = 10;

    private List<RowContainer> activeRows = new List<RowContainer>();
    private Queue<RowContainer> rowPool = new Queue<RowContainer>();
    private bool running;

    public void StartGrid()
    {
        ClearAllRows();

        for (int i = 0; i < InitialRows; i++)
        {
            float y = PlayerY - i * CellSize;
            SpawnRow(y);
        }

        running = true;
    }

    private void Update()
    {
        if (!running) return;

        float delta = ScrollSpeed * Time.deltaTime;

        for (int i = 0; i < activeRows.Count; i++)
        {
            Vector2 pos = activeRows[i].Rect.anchoredPosition;
            pos.y += delta;
            activeRows[i].Rect.anchoredPosition = pos;
        }

        for (int i = activeRows.Count - 1; i >= 0; i--)
        {
            if (activeRows[i].Rect.anchoredPosition.y > PlayerY + CellSize)
            {
                DespawnRow(activeRows[i]);
                activeRows.RemoveAt(i);
            }
        }

        float bottomY = float.MaxValue;
        for (int i = 0; i < activeRows.Count; i++)
        {
            float y = activeRows[i].Rect.anchoredPosition.y;
            if (y < bottomY) bottomY = y;
        }

        float targetBottomY = PlayerY - (InitialRows - 1) * CellSize;
        while (bottomY > targetBottomY)
        {
            bottomY -= CellSize;
            SpawnRow(bottomY);
        }
    }

    private void SpawnRow(float y)
    {
        RowContainer row;
        if (rowPool.Count > 0)
        {
            row = rowPool.Dequeue();
            row.gameObject.SetActive(true);
        }
        else
        {
            row = CreateNewRow();
        }

        Vector2 pos = row.Rect.anchoredPosition;
        pos.x = 0f;
        pos.y = y;
        row.Rect.anchoredPosition = pos;

        int[] colors = GenerateRowColors();
        row.ApplyColors(colors);
        activeRows.Add(row);
    }

    private int[] GenerateRowColors()
    {
        int[] colors = new int[Columns];
        int playerColor = Player.ColorIndex;

        int guaranteedCol = Random.Range(0, Columns);

        for (int i = 0; i < Columns; i++)
        {
            if (i == guaranteedCol) colors[i] = playerColor;
            else colors[i] = ColorPalette.GetRandomIndex();
        }

        return colors;
    }

    private void DespawnRow(RowContainer row)
    {
        row.gameObject.SetActive(false);
        rowPool.Enqueue(row);
    }

    private void ClearAllRows()
    {
        for (int i = 0; i < activeRows.Count; i++)
        {
            DespawnRow(activeRows[i]);
        }
        activeRows.Clear();
    }

    private RowContainer CreateNewRow()
    {
        GameObject rowGo = new GameObject("Row");
        rowGo.transform.SetParent(GridRoot, false);
        RectTransform rt = rowGo.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.sizeDelta = new Vector2(Columns * CellSize, CellSize);

        RowContainer row = rowGo.AddComponent<RowContainer>();
        row.Rect = rt;

        BlockCell[] cells = new BlockCell[Columns];
        float leftEdge = -Columns * CellSize * 0.5f + CellSize * 0.5f;

        for (int i = 0; i < Columns; i++)
        {
            GameObject cellGo = new GameObject("Cell_" + i);
            cellGo.transform.SetParent(rt, false);

            RectTransform crt = cellGo.AddComponent<RectTransform>();
            crt.anchorMin = new Vector2(0.5f, 0.5f);
            crt.anchorMax = new Vector2(0.5f, 0.5f);
            crt.pivot = new Vector2(0.5f, 0.5f);
            crt.sizeDelta = new Vector2(CellSize - CellGap, CellSize - CellGap);
            crt.anchoredPosition = new Vector2(leftEdge + i * CellSize, 0f);

            Image img = cellGo.AddComponent<Image>();
            img.color = Color.white;
            img.raycastTarget = false;

            BlockCell cell = cellGo.AddComponent<BlockCell>();
            cell.Image = img;
            cells[i] = cell;
        }

        row.Cells = cells;
        return row;
    }
}
