using UnityEngine;

public class HapticManager : MonoBehaviour
{
    public static HapticManager Instance { get; private set; }

    private const string PREF_HAPTIC_ON = "haptic_on";

    private bool hapticOn = true;

    public bool HapticOn
    {
        get { return hapticOn; }
        set
        {
            hapticOn = value;
            PlayerPrefs.SetInt(PREF_HAPTIC_ON, hapticOn ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        hapticOn = PlayerPrefs.GetInt(PREF_HAPTIC_ON, 1) == 1;
    }

    public void Light()
    {
        if (!hapticOn) return;
        if (Application.isMobilePlatform) Handheld.Vibrate();
    }

    public void Medium()
    {
        if (!hapticOn) return;
        if (Application.isMobilePlatform) Handheld.Vibrate();
    }

    public void Heavy()
    {
        if (!hapticOn) return;
        if (Application.isMobilePlatform) Handheld.Vibrate();
    }
}
