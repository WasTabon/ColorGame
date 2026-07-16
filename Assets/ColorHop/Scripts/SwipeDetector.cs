using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeDetector : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public float SwipeThreshold = 40f;

    public event Action OnSwipeLeft;
    public event Action OnSwipeRight;

    private Vector2 startPos;
    private bool swipeFired;
    private bool isDown;

    public void OnPointerDown(PointerEventData eventData)
    {
        isDown = true;
        swipeFired = false;
        startPos = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDown = false;
        swipeFired = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPos = eventData.position;
        swipeFired = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (swipeFired) return;

        float deltaX = eventData.position.x - startPos.x;

        if (Mathf.Abs(deltaX) >= SwipeThreshold)
        {
            if (deltaX > 0) { if (OnSwipeRight != null) OnSwipeRight(); }
            else { if (OnSwipeLeft != null) OnSwipeLeft(); }

            swipeFired = true;
            startPos = eventData.position;
            Invoke("ResetSwipeFlag", 0.05f);
        }
    }

    private void ResetSwipeFlag()
    {
        swipeFired = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        swipeFired = false;
    }
}
