using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public Button PlayButton;
    public Button SettingsButton;
    public Button StatsButton;

    private void Start()
    {
        Debug.Assert(PlayButton != null, "PlayButton not assigned!");
        Debug.Assert(SettingsButton != null, "SettingsButton not assigned!");
        Debug.Assert(StatsButton != null, "StatsButton not assigned!");

        PlayButton.onClick.AddListener(OnPlay);
        SettingsButton.onClick.AddListener(OnSettings);
        StatsButton.onClick.AddListener(OnStats);
    }

    private void OnPlay()
    {
        TransitionManager.Instance.LoadScene("Game");
    }

    private void OnSettings()
    {
        Debug.Log("Settings pressed (panel in iteration 4)");
    }

    private void OnStats()
    {
        Debug.Log("Stats pressed (panel in iteration 4)");
    }
}
