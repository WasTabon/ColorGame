using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPopup : MonoBehaviour
{
    public PopupBase Popup;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI BestText;
    public Button RestartButton;
    public Button MenuButton;

    public event Action OnRestart;
    public event Action OnMenu;

    private void Awake()
    {
        Debug.Assert(Popup != null, "Popup not assigned!");
        Debug.Assert(ScoreText != null, "ScoreText not assigned!");
        Debug.Assert(BestText != null, "BestText not assigned!");
        Debug.Assert(RestartButton != null, "RestartButton not assigned!");
        Debug.Assert(MenuButton != null, "MenuButton not assigned!");

        RestartButton.onClick.AddListener(HandleRestart);
        MenuButton.onClick.AddListener(HandleMenu);
        gameObject.SetActive(false);
    }

    public void Show(int score, int best)
    {
        ScoreText.text = "SCORE " + score;
        BestText.text = "BEST " + best;
        Popup.Open();
    }

    private void HandleRestart()
    {
        if (SoundManager.Instance != null) SoundManager.Instance.PlayButton();
        Popup.Close(() =>
        {
            if (OnRestart != null) OnRestart();
        });
    }

    private void HandleMenu()
    {
        if (SoundManager.Instance != null) SoundManager.Instance.PlayButton();
        Popup.Close(() =>
        {
            if (OnMenu != null) OnMenu();
        });
    }
}
