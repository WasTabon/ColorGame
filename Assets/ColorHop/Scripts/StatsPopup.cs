using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsPopup : MonoBehaviour
{
    public PopupBase Popup;
    public TextMeshProUGUI BestScoreText;
    public TextMeshProUGUI TotalGamesText;
    public TextMeshProUGUI TotalBlocksText;
    public TextMeshProUGUI BestComboText;
    public Button CloseButton;

    private void Awake()
    {
        Debug.Assert(Popup != null, "Popup not assigned!");
        Debug.Assert(BestScoreText != null, "BestScoreText not assigned!");
        Debug.Assert(TotalGamesText != null, "TotalGamesText not assigned!");
        Debug.Assert(TotalBlocksText != null, "TotalBlocksText not assigned!");
        Debug.Assert(BestComboText != null, "BestComboText not assigned!");
        Debug.Assert(CloseButton != null, "CloseButton not assigned!");

        CloseButton.onClick.AddListener(HandleClose);
        gameObject.SetActive(false);
    }

    public void Show()
    {
        Popup.Open();

        AnimateCount(BestScoreText, "BEST SCORE  ", StatsManager.GetBestScore());
        AnimateCount(TotalGamesText, "GAMES PLAYED  ", StatsManager.GetTotalGames());
        AnimateCount(TotalBlocksText, "BLOCKS BROKEN  ", StatsManager.GetTotalBlocksBroken());
        AnimateCount(BestComboText, "BEST COMBO  ", StatsManager.GetBestCombo());
    }

    private void AnimateCount(TextMeshProUGUI label, string prefix, int target)
    {
        label.text = prefix + "0";
        int current = 0;
        DOTween.To(() => current, x =>
        {
            current = x;
            label.text = prefix + x;
        }, target, 0.6f).SetEase(Ease.OutQuad).SetDelay(0.1f);
    }

    public void Hide()
    {
        Popup.Close(null);
    }

    private void HandleClose()
    {
        if (SoundManager.Instance != null) SoundManager.Instance.PlayButton();
        Hide();
    }
}
