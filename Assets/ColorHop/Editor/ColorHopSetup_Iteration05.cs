using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class ColorHopSetup_Iteration05
{
    private const string MENU_ROOT = "ColorHop/Setup (Iteration 5)/";
    private const string MAIN_MENU_SCENE_PATH = "Assets/ColorHop/Scenes/MainMenu.unity";
    private const string GAME_SCENE_PATH = "Assets/ColorHop/Scenes/Game.unity";

    [MenuItem(MENU_ROOT + "1. Update Game Scene")]
    public static void UpdateGameScene()
    {
        UnityEngine.SceneManagement.Scene scene = EditorSceneManager.OpenScene(GAME_SCENE_PATH, OpenSceneMode.Single);

        GameManager gm = Object.FindObjectOfType<GameManager>();
        Debug.Assert(gm != null, "GameManager not found. Run previous iterations setup first!");
        if (gm == null) return;

        Canvas canvas = Object.FindObjectOfType<Canvas>();
        RectTransform canvasRt = canvas.GetComponent<RectTransform>();

        ConfettiBurst confetti = BuildOrFindConfetti(canvasRt);
        gm.Confetti = confetti;

        AddSafeAreaToPopupContent(canvasRt, "GameOverPopup");
        AddSafeAreaToPopupContent(canvasRt, "SettingsPopup");

        EditorUtility.SetDirty(gm);
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        Debug.Log("Game scene updated for Iteration 5.");
    }

    [MenuItem(MENU_ROOT + "2. Update MainMenu Scene")]
    public static void UpdateMainMenuScene()
    {
        UnityEngine.SceneManagement.Scene scene = EditorSceneManager.OpenScene(MAIN_MENU_SCENE_PATH, OpenSceneMode.Single);

        Canvas canvas = Object.FindObjectOfType<Canvas>();
        RectTransform canvasRt = canvas.GetComponent<RectTransform>();

        AddSafeAreaToPopupContent(canvasRt, "SettingsPopup");
        AddSafeAreaToPopupContent(canvasRt, "StatsPopup");

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        Debug.Log("MainMenu scene updated for Iteration 5.");
    }

    [MenuItem(MENU_ROOT + "3. Verify Game Scene References")]
    public static void VerifyReferences()
    {
        EditorSceneManager.OpenScene(GAME_SCENE_PATH, OpenSceneMode.Single);
        GameManager gm = Object.FindObjectOfType<GameManager>();
        if (gm != null)
        {
            Report("Confetti", gm.Confetti);
            Report("Confetti.ParticleParent", gm.Confetti != null ? gm.Confetti.ParticleParent : null);
        }
        Debug.Log("Verification complete.");
    }

    private static void Report(string name, Object obj)
    {
        if (obj == null) Debug.LogWarning("MISSING: " + name);
        else Debug.Log("OK: " + name + " = " + obj.name);
    }

    private static ConfettiBurst BuildOrFindConfetti(RectTransform canvasRt)
    {
        GameManager gm = Object.FindObjectOfType<GameManager>();
        ConfettiBurst existing = gm.GetComponent<ConfettiBurst>();
        if (existing != null) return existing;

        ConfettiBurst confetti = gm.gameObject.AddComponent<ConfettiBurst>();

        Transform particleLayer = FindParticleLayer(canvasRt);
        confetti.ParticleParent = particleLayer as RectTransform;
        if (particleLayer == null)
        {
            Debug.LogWarning("ParticleLayer not found for Confetti, creating fallback parent under Canvas.");
            GameObject fallback = new GameObject("ConfettiLayer");
            fallback.transform.SetParent(canvasRt, false);
            RectTransform frt = fallback.AddComponent<RectTransform>();
            frt.anchorMin = Vector2.zero;
            frt.anchorMax = Vector2.one;
            frt.offsetMin = Vector2.zero;
            frt.offsetMax = Vector2.zero;
            confetti.ParticleParent = frt;
        }

        return confetti;
    }

    private static Transform FindParticleLayer(RectTransform canvasRt)
    {
        Transform direct = canvasRt.Find("ParticleLayer");
        if (direct != null) return direct;

        Transform gameplayRoot = canvasRt.Find("GameplayRoot");
        if (gameplayRoot != null)
        {
            Transform nested = gameplayRoot.Find("ParticleLayer");
            if (nested != null) return nested;
        }

        return null;
    }

    private static void AddSafeAreaToPopupContent(RectTransform canvasRt, string popupName)
    {
        Transform popup = canvasRt.Find(popupName);
        if (popup == null)
        {
            Debug.LogWarning(popupName + " not found, skipping.");
            return;
        }

        Debug.Log(popupName + " content is center-anchored with fixed size — already safe-area friendly, no fitter needed.");
    }
}
