using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GridManager Grid;
    public PlayerCube Player;
    public DragDetector DragDetector;

    private float dragStartPointerX;
    private float dragStartCubeX;

    private void Start()
    {
        Debug.Assert(Grid != null, "Grid not assigned!");
        Debug.Assert(Player != null, "Player not assigned!");
        Debug.Assert(DragDetector != null, "DragDetector not assigned!");

        Player.SetColor(0);
        Player.SetColumnInstant(2, Grid.CellSize, Grid.Columns);

        DragDetector.OnDragBegin -= HandleDragBegin;
        DragDetector.OnDragBegin += HandleDragBegin;
        DragDetector.OnDragMove -= HandleDragMove;
        DragDetector.OnDragMove += HandleDragMove;
        DragDetector.OnDragEnd -= HandleDragEnd;
        DragDetector.OnDragEnd += HandleDragEnd;

        Grid.StartGrid();
    }

    private void HandleDragBegin(float pointerCanvasX)
    {
        dragStartPointerX = pointerCanvasX;
        dragStartCubeX = Player.Rect.anchoredPosition.x;
    }

    private void HandleDragMove(float pointerCanvasX)
    {
        float delta = pointerCanvasX - dragStartPointerX;
        float targetX = dragStartCubeX + delta;

        float halfRange = (Grid.Columns - 1) * Grid.CellSize * 0.5f;
        targetX = Mathf.Clamp(targetX, -halfRange, halfRange);

        Player.SetXInstant(targetX);
    }

    private void HandleDragEnd()
    {
        Player.SnapToNearestColumn(Grid.CellSize, Grid.Columns);
    }

    private void OnDestroy()
    {
        if (DragDetector != null)
        {
            DragDetector.OnDragBegin -= HandleDragBegin;
            DragDetector.OnDragMove -= HandleDragMove;
            DragDetector.OnDragEnd -= HandleDragEnd;
        }
    }
}
