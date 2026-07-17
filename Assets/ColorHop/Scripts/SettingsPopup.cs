using UnityEngine;
using UnityEngine.UI;

public class SettingsPopup : MonoBehaviour
{
    public PopupBase Popup;
    public Toggle SoundToggle;
    public Toggle MusicToggle;
    public Toggle HapticToggle;
    public Button CloseButton;

    private bool initializing;

    private void Awake()
    {
        Debug.Assert(Popup != null, "Popup not assigned!");
        Debug.Assert(SoundToggle != null, "SoundToggle not assigned!");
        Debug.Assert(MusicToggle != null, "MusicToggle not assigned!");
        Debug.Assert(HapticToggle != null, "HapticToggle not assigned!");
        Debug.Assert(CloseButton != null, "CloseButton not assigned!");

        CloseButton.onClick.AddListener(HandleClose);
        SoundToggle.onValueChanged.AddListener(HandleSoundChanged);
        MusicToggle.onValueChanged.AddListener(HandleMusicChanged);
        HapticToggle.onValueChanged.AddListener(HandleHapticChanged);
    }

    public void Show()
    {
        initializing = true;
        if (SoundManager.Instance != null) SoundToggle.isOn = SoundManager.Instance.SfxOn;
        if (SoundManager.Instance != null) MusicToggle.isOn = SoundManager.Instance.MusicOn;
        if (HapticManager.Instance != null) HapticToggle.isOn = HapticManager.Instance.HapticOn;
        initializing = false;

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

    private void HandleSoundChanged(bool value)
    {
        if (initializing) return;
        if (SoundManager.Instance != null) SoundManager.Instance.SfxOn = value;
        if (value && SoundManager.Instance != null) SoundManager.Instance.PlayButton();
    }

    private void HandleMusicChanged(bool value)
    {
        if (initializing) return;
        if (SoundManager.Instance != null) SoundManager.Instance.MusicOn = value;
    }

    private void HandleHapticChanged(bool value)
    {
        if (initializing) return;
        if (HapticManager.Instance != null) HapticManager.Instance.HapticOn = value;
        if (value && HapticManager.Instance != null) HapticManager.Instance.Light();
    }
}
