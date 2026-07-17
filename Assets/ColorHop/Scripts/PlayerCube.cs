using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCube : MonoBehaviour
{
    public Image Image;
    public RectTransform Rect;

    public int CurrentColumn { get; private set; } = 2;

    private int colorIndex;
    private Tween snapTween;
    private Tween squashTween;
    private Vector3 baseScale = Vector3.one;

    public int ColorIndex { get { return colorIndex; } }

    public void SetColor(int index)
    {
        colorIndex = index;
        Image.color = ColorPalette.Colors[index];
    }

    public void SetColumnInstant(int column, float columnWidth, int totalColumns)
    {
        CurrentColumn = column;
        Vector2 pos = Rect.anchoredPosition;
        pos.x = ColumnToX(column, columnWidth, totalColumns);
        Rect.anchoredPosition = pos;
    }

    public void SetXInstant(float x)
    {
        if (snapTween != null) snapTween.Kill();
        Vector2 pos = Rect.anchoredPosition;
        pos.x = x;
        Rect.anchoredPosition = pos;
    }

    public void SnapToNearestColumn(float columnWidth, int totalColumns)
    {
        float currentX = Rect.anchoredPosition.x;
        float halfWidth = columnWidth * totalColumns * 0.5f;
        float leftEdge = -halfWidth + columnWidth * 0.5f;

        int nearest = Mathf.RoundToInt((currentX - leftEdge) / columnWidth);
        nearest = Mathf.Clamp(nearest, 0, totalColumns - 1);

        CurrentColumn = nearest;
        float targetX = ColumnToX(nearest, columnWidth, totalColumns);

        if (snapTween != null) snapTween.Kill();
        snapTween = Rect.DOAnchorPosX(targetX, 0.12f).SetEase(Ease.OutBack);

        PlaySquash();
    }

    public void PunchSuccess()
    {
        if (squashTween != null) squashTween.Kill();
        Rect.localScale = baseScale;
        squashTween = Rect.DOPunchScale(Vector3.one * 0.35f, 0.35f, 8, 0.6f);
    }

    public void PlaySwitchEffect(int newColorIndex)
    {
        if (squashTween != null) squashTween.Kill();
        Rect.localScale = baseScale;
        Sequence seq = DOTween.Sequence();
        seq.Append(Rect.DOScale(0f, 0.15f).SetEase(Ease.InBack));
        seq.AppendCallback(() => { SetColor(newColorIndex); });
        seq.Append(Rect.DOScale(baseScale, 0.25f).SetEase(Ease.OutBack));
        squashTween = seq;
    }

    private void PlaySquash()
    {
        if (squashTween != null) squashTween.Kill();
        Rect.localScale = new Vector3(1.1f, 0.9f, 1f);
        squashTween = Rect.DOScale(baseScale, 0.18f).SetEase(Ease.OutBack);
    }

    private float ColumnToX(int column, float columnWidth, int totalColumns)
    {
        float totalWidth = columnWidth * totalColumns;
        float leftEdge = -totalWidth * 0.5f + columnWidth * 0.5f;
        return leftEdge + column * columnWidth;
    }

    private void OnDisable()
    {
        if (snapTween != null) snapTween.Kill();
        if (squashTween != null) squashTween.Kill();
        Rect.localScale = baseScale;
    }
}
