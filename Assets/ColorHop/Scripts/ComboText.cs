using DG.Tweening;
using TMPro;
using UnityEngine;

public class ComboText : MonoBehaviour
{
    public TextMeshProUGUI Label;
    public RectTransform Rect;

    private Sequence sequence;

    public void Show(string text)
    {
        Debug.Assert(Label != null, "Label not assigned on ComboText!");
        Debug.Assert(Rect != null, "Rect not assigned on ComboText!");

        if (sequence != null) sequence.Kill();

        Label.text = text;
        Rect.localScale = Vector3.zero;
        Label.alpha = 1f;
        gameObject.SetActive(true);

        sequence = DOTween.Sequence();
        sequence.Append(Rect.DOScale(1.15f, 0.2f).SetEase(Ease.OutBack));
        sequence.Append(Rect.DOScale(1f, 0.1f).SetEase(Ease.OutQuad));
        sequence.AppendInterval(0.5f);
        sequence.Append(Label.DOFade(0f, 0.3f).SetEase(Ease.InQuad));
        sequence.Join(Rect.DOScale(0.8f, 0.3f).SetEase(Ease.InQuad));
        sequence.OnComplete(() => { gameObject.SetActive(false); });
    }

    private void OnDisable()
    {
        if (sequence != null) sequence.Kill();
    }
}
