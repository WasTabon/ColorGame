using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class ColorHopSetup_Iteration03
{
    private const string MENU_ROOT = "ColorHop/Setup (Iteration 3)/";
    private const string GAME_SCENE_PATH = "Assets/ColorHop/Scenes/Game.unity";

    [MenuItem(MENU_ROOT + "1. Update Game Scene")]
    public static void UpdateGameScene()
    {
        UnityEngine.SceneManagement.Scene scene = EditorSceneManager.OpenScene(GAME_SCENE_PATH, OpenSceneMode.Single);

        GameManager gm = Object.FindObjectOfType<GameManager>();
        Debug.Assert(gm != null, "GameManager not found in Game scene. Run previous iterations setup first!");
        if (gm == null) return;

        Canvas canvas = Object.FindObjectOfType<Canvas>();
        Debug.Assert(canvas != null, "Canvas not found!");
        RectTransform canvasRt = canvas.GetComponent<RectTransform>();

        ComboManager combo = gm.GetComponent<ComboManager>();
        if (combo == null) combo = gm.gameObject.AddComponent<ComboManager>();

        PlayerColorSwitcher switcher = gm.GetComponent<PlayerColorSwitcher>();
        if (switcher == null) switcher = gm.gameObject.AddComponent<PlayerColorSwitcher>();
        switcher.Player = gm.Player;

        RectTransform gameplayRoot = FindOrCreateGameplayRoot(canvas.transform, canvasRt);

        ScreenShaker shaker = gm.GetComponent<ScreenShaker>();
        if (shaker == null) shaker = gm.gameObject.AddComponent<ScreenShaker>();
        shaker.ShakeTarget = gameplayRoot;

        ComboText comboText = FindOrCreateComboText(canvasRt);

        gm.ComboManager = combo;
        gm.ColorSwitcher = switcher;
        gm.ScreenShaker = shaker;
        gm.ComboText = comboText;

        EditorUtility.SetDirty(gm);
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        Debug.Log("Game scene updated for Iteration 3.");
    }

    [MenuItem(MENU_ROOT + "2. Verify Game Scene References")]
    public static void VerifyReferences()
    {
        if (SceneManager.GetActiveScene().path != GAME_SCENE_PATH)
        {
            EditorSceneManager.OpenScene(GAME_SCENE_PATH, OpenSceneMode.Single);
        }
        GameManager gm = Object.FindObjectOfType<GameManager>();
        Debug.Assert(gm != null, "GameManager not found!");
        if (gm == null) return;

        Report("ComboManager", gm.ComboManager);
        Report("ColorSwitcher", gm.ColorSwitcher);
        Report("ColorSwitcher.Player", gm.ColorSwitcher != null ? gm.ColorSwitcher.Player : null);
        Report("ScreenShaker", gm.ScreenShaker);
        Report("ScreenShaker.ShakeTarget", gm.ScreenShaker != null ? gm.ScreenShaker.ShakeTarget : null);
        Report("ComboText", gm.ComboText);
        if (gm.ComboText != null)
        {
            Report("ComboText.Label", gm.ComboText.Label);
            Report("ComboText.Rect", gm.ComboText.Rect);
        }
        Debug.Log("Verification complete.");
    }

    private static void Report(string name, Object obj)
    {
        if (obj == null) Debug.LogWarning("MISSING: " + name);
        else Debug.Log("OK: " + name + " = " + obj.name);
    }

    private static RectTransform FindOrCreateGameplayRoot(Transform canvasT, RectTransform canvasRt)
    {
        Transform existing = canvasT.Find("GameplayRoot");
        if (existing != null) return existing.GetComponent<RectTransform>();

        GameObject go = new GameObject("GameplayRoot");
        go.transform.SetParent(canvasT, false);
        RectTransform rt = go.AddComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        Transform gridRoot = canvasT.Find("GridRoot");
        Transform playerCube = canvasT.Find("PlayerCube");
        Transform particleLayer = canvasT.Find("ParticleLayer");

        Debug.Assert(gridRoot != null, "GridRoot not found under Canvas!");
        Debug.Assert(playerCube != null, "PlayerCube not found under Canvas!");

        if (gridRoot != null)
        {
            int idx = gridRoot.GetSiblingIndex();
            gridRoot.SetParent(rt, true);
            go.transform.SetSiblingIndex(idx);
        }
        if (particleLayer != null) particleLayer.SetParent(rt, true);
        if (playerCube != null) playerCube.SetParent(rt, true);

        return rt;
    }

    private static ComboText FindOrCreateComboText(RectTransform canvasRt)
    {
        Transform existing = canvasRt.Find("ComboText");
        if (existing != null)
        {
            ComboText ex = existing.GetComponent<ComboText>();
            if (ex != null) return ex;
        }

        GameObject go = new GameObject("ComboText");
        go.transform.SetParent(canvasRt, false);
        RectTransform rt = go.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = new Vector2(0f, 250f);
        rt.sizeDelta = new Vector2(800f, 200f);

        TextMeshProUGUI tmp = go.AddComponent<TextMeshProUGUI>();
        tmp.text = "GREAT!";
        tmp.fontSize = 100f;
        tmp.fontStyle = FontStyles.Bold;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.white;
        tmp.raycastTarget = false;

        ComboText comboText = go.AddComponent<ComboText>();
        comboText.Label = tmp;
        comboText.Rect = rt;

        go.SetActive(false);
        return comboText;
    }
}
