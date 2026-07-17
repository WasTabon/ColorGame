using System;
using UnityEngine;
using UnityEngine.UI;

public class ExitConfirmPopup : MonoBehaviour
{
    public PopupBase Popup;
    public Button ConfirmButton;
    public Button CancelButton;

    public event Action OnConfirmExit;

    private void Awake()
    {
        Debug.Assert(Popup != null, "Popup not assigned!");
        Debug.Assert(ConfirmButton != null, "ConfirmButton not assigned!");
        Debug.Assert(CancelButton != null, "CancelButton not assigned!");

        ConfirmButton.onClick.AddListener(HandleConfirm);
        CancelButton.onClick.AddListener(HandleCancel);
    }

    public void Show()
    {
        Popup.Open();
    }

    private void HandleConfirm()
    {
        if (SoundManager.Instance != null) SoundManager.Instance.PlayButton();
        Popup.Close(() =>
        {
            if (OnConfirmExit != null) OnConfirmExit();
        });
    }

    private void HandleCancel()
    {
        if (SoundManager.Instance != null) SoundManager.Instance.PlayButton();
        Popup.Close(null);
    }
}
