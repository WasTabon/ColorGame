using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PopupBase : MonoBehaviour
{
    public RectTransform Content;
    public Image Backdrop;
    public float BackdropAlpha = 0.6f;
    public float OpenDuration = 0.3f;
    public float CloseDuration = 0.22f;

    private Tween contentTween;
    private Tween backdropTween;

    public void Open()
    {
        gameObject.SetActive(true);
        if (contentTween != null) contentTween.Kill();
        if (backdropTween != null) backdropTween.Kill();

        Content.localScale = Vector3.zero;
        Backdrop.color = new Color(Backdrop.color.r, Backdrop.color.g, Backdrop.color.b, 0f);

        contentTween = Content.DOScale(1f, OpenDuration).SetEase(Ease.OutBack);
        backdropTween = Backdrop.DOFade(BackdropAlpha, OpenDuration).SetEase(Ease.OutQuad);
    }

    public void Close(System.Action onClosed)
    {
        if (contentTween != null) contentTween.Kill();
        if (backdropTween != null) backdropTween.Kill();

        contentTween = Content.DOScale(0f, CloseDuration).SetEase(Ease.InBack);
        backdropTween = Backdrop.DOFade(0f, CloseDuration).SetEase(Ease.InQuad).OnComplete(() =>
        {
            gameObject.SetActive(false);
            if (onClosed != null) onClosed();
        });
    }
}
