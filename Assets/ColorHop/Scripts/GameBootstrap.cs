using UnityEngine;

public class GameBootstrap : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = 60;

        if (SoundManager.Instance == null)
        {
            GameObject go = new GameObject("SoundManager");
            go.AddComponent<SoundManager>();
        }

        if (HapticManager.Instance == null)
        {
            GameObject go = new GameObject("HapticManager");
            go.AddComponent<HapticManager>();
        }

        if (TransitionManager.Instance == null)
        {
            GameObject go = new GameObject("TransitionManager");
            go.AddComponent<TransitionManager>();
        }
    }
}
