using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class StartCountdown : MonoBehaviour
{
    public TextMeshProUGUI CountdownText;
    public CanvasGroup CanvasGroup;

    public event Action OnCountdownFinished;

    public void PlayCountdown()
    {
        Debug.Assert(CountdownText != null, "CountdownText not assigned!");
        Debug.Assert(CanvasGroup != null, "CanvasGroup not assigned!");

        gameObject.SetActive(true);
        CanvasGroup.alpha = 1f;

        Sequence seq = DOTween.Sequence();
        seq.AppendCallback(() => ShowNumber("3"));
        seq.AppendInterval(0.72f);
        seq.AppendCallback(() => ShowNumber("2"));
        seq.AppendInterval(0.72f);
        seq.AppendCallback(() => ShowNumber("1"));
        seq.AppendInterval(0.72f);
        seq.AppendCallback(() => ShowNumber("GO!"));
        seq.AppendInterval(0.35f);
        seq.Append(CanvasGroup.DOFade(0f, 0.2f));
        seq.OnComplete(() =>
        {
            gameObject.SetActive(false);
            if (OnCountdownFinished != null) OnCountdownFinished();
        });
    }

    private void ShowNumber(string text)
    {
        CountdownText.text = text;
        CountdownText.rectTransform.localScale = Vector3.one * 0.4f;
        CountdownText.alpha = 1f;

        Sequence popSeq = DOTween.Sequence();
        popSeq.Append(CountdownText.rectTransform.DOScale(1f, 0.25f).SetEase(Ease.OutBack));

        if (SoundManager.Instance != null) SoundManager.Instance.PlayTap();
    }
}
