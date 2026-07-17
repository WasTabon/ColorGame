using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    public RectTransform GridRoot;
    public PlayerCube Player;

    public int Columns = 5;
    public float CellSize = 216f;
    public float CellGap = 6f;
    public float PlayerY = 550f;
    public int InitialRows = 10;
    public float RowSnapThreshold = 40f;
    public float AdvanceDuration = 0.25f;

    public float LastAdvanceDuration { get { return AdvanceDuration; } }

    private List<RowContainer> activeRows = new List<RowContainer>();
    private Queue<RowContainer> rowPool = new Queue<RowContainer>();

    public void StartGrid()
    {
        ClearAllRows();

        for (int i = 0; i < InitialRows; i++)
        {
            float y = PlayerY - (i + 1) * CellSize;
            SpawnRow(y);
        }
    }

    public RowContainer FindTopRow()
    {
        RowContainer top = null;
        float minDist = RowSnapThreshold;
        float targetY = PlayerY - CellSize;
        for (int i = 0; i < activeRows.Count; i++)
        {
            float dist = Mathf.Abs(activeRows[i].Rect.anchoredPosition.y - targetY);
            if (dist < minDist)
            {
                minDist = dist;
                top = activeRows[i];
            }
        }
        return top;
    }

    public int GetColumnAtX(float x)
    {
        float halfWidth = Columns * CellSize * 0.5f;
        float leftEdge = -halfWidth + CellSize * 0.5f;
        int col = Mathf.RoundToInt((x - leftEdge) / CellSize);
        return Mathf.Clamp(col, 0, Columns - 1);
    }

    public void OnRowMatched(RowContainer matchedRow)
    {
        int idx = activeRows.IndexOf(matchedRow);
        if (idx < 0)
        {
            Debug.LogWarning("OnRowMatched called with row not in active list");
            return;
        }
        activeRows.RemoveAt(idx);

        float minY = float.MaxValue;
        for (int i = 0; i < activeRows.Count; i++)
        {
            float y = activeRows[i].Rect.anchoredPosition.y;
            if (y < minY) minY = y;
        }

        if (activeRows.Count == 0) minY = PlayerY - CellSize;

        float newRowY = minY - CellSize;
        SpawnRow(newRowY);

        for (int i = 0; i < activeRows.Count; i++)
        {
            Vector2 pos = activeRows[i].Rect.anchoredPosition;
            Vector2 target = new Vector2(pos.x, pos.y + CellSize);
            activeRows[i].Rect.DOKill(false);
            activeRows[i].Rect.DOAnchorPos(target, AdvanceDuration).SetEase(Ease.OutQuad);
        }
    }

    public void ReturnRowToPool(RowContainer row)
    {
        DespawnRow(row);
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

        row.Rect.DOKill(false);
        row.Rect.localScale = Vector3.one;
        Vector2 pos = row.Rect.anchoredPosition;
        pos.x = 0f;
        pos.y = y;
        row.Rect.anchoredPosition = pos;

        int[] colors = GenerateRowColors();
        row.ApplyColors(colors);
        activeRows.Add(row);
    }

    public void EnsureColorInTopRow(int colorIndex)
    {
        RowContainer top = FindTopRow();
        if (top == null) return;
        if (top.ContainsColor(colorIndex)) return;

        int col = Random.Range(0, Columns);
        top.Cells[col].SetColor(colorIndex);
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
        row.Rect.DOKill(false);
        row.gameObject.SetActive(false);
        row.Rect.localScale = Vector3.one;
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
