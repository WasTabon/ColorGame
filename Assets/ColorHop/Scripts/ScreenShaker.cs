using DG.Tweening;
using UnityEngine;

public class ScreenShaker : MonoBehaviour
{
    public RectTransform ShakeTarget;

    private Vector2 originalPos;
    private Tween shakeTween;

    private void Awake()
    {
        if (ShakeTarget != null) originalPos = ShakeTarget.anchoredPosition;
    }

    public void Shake(float strength, float duration)
    {
        Debug.Assert(ShakeTarget != null, "ShakeTarget not assigned on ScreenShaker!");
        if (ShakeTarget == null) return;

        if (shakeTween != null) shakeTween.Kill();
        ShakeTarget.anchoredPosition = originalPos;
        shakeTween = ShakeTarget.DOShakeAnchorPos(duration, strength, 20, 90, false, true)
            .OnComplete(() => { ShakeTarget.anchoredPosition = originalPos; });
    }

    public void ShakeSmall()
    {
        Shake(12f, 0.2f);
    }

    public void ShakeMedium()
    {
        Shake(22f, 0.3f);
    }

    public void ShakeBig()
    {
        Shake(35f, 0.4f);
    }
}
