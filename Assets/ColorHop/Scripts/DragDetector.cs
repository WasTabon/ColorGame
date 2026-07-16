using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDetector : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IBeginDragHandler
{
    public RectTransform CanvasRect;

    public event Action<float> OnDragBegin;
    public event Action<float> OnDragMove;
    public event Action OnDragEnd;

    private bool isDown;

    public void OnPointerDown(PointerEventData eventData)
    {
        isDown = true;
        float canvasX = ScreenToCanvasX(eventData.position);
        if (OnDragBegin != null) OnDragBegin(canvasX);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDown) return;
        float canvasX = ScreenToCanvasX(eventData.position);
        if (OnDragMove != null) OnDragMove(canvasX);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isDown) return;
        isDown = false;
        if (OnDragEnd != null) OnDragEnd();
    }

    private float ScreenToCanvasX(Vector2 screenPos)
    {
        Debug.Assert(CanvasRect != null, "CanvasRect not assigned on DragDetector!");
        Vector2 local;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(CanvasRect, screenPos, null, out local);
        return local.x;
    }
}
