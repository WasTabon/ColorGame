using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public Button PlayButton;
    public Button SettingsButton;
    public Button StatsButton;
    public SettingsPopup SettingsPopup;
    public StatsPopup StatsPopup;

    private void Start()
    {
        Debug.Assert(PlayButton != null, "PlayButton not assigned!");
        Debug.Assert(SettingsButton != null, "SettingsButton not assigned!");
        Debug.Assert(StatsButton != null, "StatsButton not assigned!");
        Debug.Assert(SettingsPopup != null, "SettingsPopup not assigned!");
        Debug.Assert(StatsPopup != null, "StatsPopup not assigned!");

        PlayButton.onClick.AddListener(OnPlay);
        SettingsButton.onClick.AddListener(OnSettings);
        StatsButton.onClick.AddListener(OnStats);
    }

    private void OnPlay()
    {
        if (SoundManager.Instance != null) SoundManager.Instance.PlayButton();
        TransitionManager.Instance.LoadScene("Game");
    }

    private void OnSettings()
    {
        if (SoundManager.Instance != null) SoundManager.Instance.PlayButton();
        SettingsPopup.Show();
    }

    private void OnStats()
    {
        if (SoundManager.Instance != null) SoundManager.Instance.PlayButton();
        StatsPopup.Show();
    }
}
