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
        BestScoreText.text = "BEST SCORE  " + StatsManager.GetBestScore();
        TotalGamesText.text = "GAMES PLAYED  " + StatsManager.GetTotalGames();
        TotalBlocksText.text = "BLOCKS BROKEN  " + StatsManager.GetTotalBlocksBroken();
        BestComboText.text = "BEST COMBO  " + StatsManager.GetBestCombo();

        Popup.Open();
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
