using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonAnimator : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    private Vector3 originalScale;
    private Tween currentTween;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        originalScale = transform.localScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!button.interactable) return;
        if (currentTween != null) currentTween.Kill();
        currentTween = transform.DOScale(originalScale * 0.92f, 0.08f).SetEase(Ease.OutQuad);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!button.interactable) return;
        if (currentTween != null) currentTween.Kill();
        currentTween = transform.DOScale(originalScale, 0.15f).SetEase(Ease.OutBack);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!button.interactable) return;
        if (currentTween != null) currentTween.Kill();
        currentTween = transform.DOScale(originalScale, 0.15f).SetEase(Ease.OutBack);
    }

    private void OnDisable()
    {
        if (currentTween != null) currentTween.Kill();
        transform.localScale = originalScale;
    }
}
