using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public TextMeshProUGUI ScoreText;
    public Image TimerBarFill;
    public TextMeshProUGUI TimerText;
    public Color TimerNormalColor = new Color(0.29f, 0.56f, 0.89f, 1f);
    public Color TimerLowColor = new Color(0.96f, 0.26f, 0.51f, 1f);

    private int displayedScore;
    private Tween scoreTween;
    private Tween scorePunchTween;

    public void Initialize()
    {
        displayedScore = 0;
        SetScoreImmediate(0);
        SetTimerImmediate(1f, 3f);
    }

    public void SetScoreAnimated(int newScore)
    {
        if (scoreTween != null) scoreTween.Kill();
        int from = displayedScore;
        scoreTween = DOTween.To(() => from, x =>
        {
            from = x;
            ScoreText.text = x.ToString();
        }, newScore, 0.25f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            displayedScore = newScore;
        });

        PunchScoreScale();
    }

    public void SetScoreImmediate(int score)
    {
        displayedScore = score;
        ScoreText.text = score.ToString();
    }

    public void UpdateTimer(float current, float max)
    {
        float fill = max > 0f ? Mathf.Clamp01(current / max) : 0f;
        TimerBarFill.fillAmount = fill;

        if (fill < 0.3f) TimerBarFill.color = TimerLowColor;
        else TimerBarFill.color = TimerNormalColor;

        TimerText.text = current.ToString("0.0");
    }

    private void SetTimerImmediate(float fill, float value)
    {
        TimerBarFill.fillAmount = fill;
        TimerBarFill.color = TimerNormalColor;
        TimerText.text = value.ToString("0.0");
    }

    private void PunchScoreScale()
    {
        if (scorePunchTween != null) scorePunchTween.Kill();
        ScoreText.rectTransform.localScale = Vector3.one;
        scorePunchTween = ScoreText.rectTransform.DOPunchScale(Vector3.one * 0.25f, 0.3f, 6, 0.5f);
    }
}
