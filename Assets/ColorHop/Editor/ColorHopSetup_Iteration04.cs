using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class ColorHopSetup_Iteration04
{
    private const string MENU_ROOT = "ColorHop/Setup (Iteration 4)/";
    private const string MAIN_MENU_SCENE_PATH = "Assets/ColorHop/Scenes/MainMenu.unity";
    private const string GAME_SCENE_PATH = "Assets/ColorHop/Scenes/Game.unity";

    [MenuItem(MENU_ROOT + "1. Update MainMenu Scene")]
    public static void UpdateMainMenuScene()
    {
        UnityEngine.SceneManagement.Scene scene = EditorSceneManager.OpenScene(MAIN_MENU_SCENE_PATH, OpenSceneMode.Single);

        MainMenuController controller = Object.FindObjectOfType<MainMenuController>();
        Debug.Assert(controller != null, "MainMenuController not found. Run Iteration 1 setup first!");
        if (controller == null) return;

        Canvas canvas = Object.FindObjectOfType<Canvas>();
        RectTransform canvasRt = canvas.GetComponent<RectTransform>();

        SettingsPopup settingsPopup = BuildOrFindSettingsPopup(canvasRt);
        StatsPopup statsPopup = BuildOrFindStatsPopup(canvasRt);

        controller.SettingsPopup = settingsPopup;
        controller.StatsPopup = statsPopup;

        EditorUtility.SetDirty(controller);
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        Debug.Log("MainMenu scene updated for Iteration 4.");
    }

    [MenuItem(MENU_ROOT + "2. Update Game Scene")]
    public static void UpdateGameScene()
    {
        UnityEngine.SceneManagement.Scene scene = EditorSceneManager.OpenScene(GAME_SCENE_PATH, OpenSceneMode.Single);

        GameManager gm = Object.FindObjectOfType<GameManager>();
        Debug.Assert(gm != null, "GameManager not found. Run previous iterations setup first!");
        if (gm == null) return;

        Canvas canvas = Object.FindObjectOfType<Canvas>();
        RectTransform canvasRt = canvas.GetComponent<RectTransform>();
        Transform safeArea = canvas.transform.Find("SafeArea");
        Transform topBar = safeArea.Find("TopBar");

        SettingsPopup settingsPopup = BuildOrFindSettingsPopup(canvasRt);

        Button settingsBtn = BuildOrFindTopBarSettingsButton(topBar);

        StartCountdown countdown = BuildOrFindCountdown(canvasRt);

        gm.SettingsPopup = settingsPopup;
        gm.SettingsButton = settingsBtn;
        gm.StartCountdown = countdown;

        EditorUtility.SetDirty(gm);
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        Debug.Log("Game scene updated for Iteration 4.");
    }

    [MenuItem(MENU_ROOT + "3. Verify Scenes References")]
    public static void VerifyReferences()
    {
        EditorSceneManager.OpenScene(MAIN_MENU_SCENE_PATH, OpenSceneMode.Single);
        MainMenuController mc = Object.FindObjectOfType<MainMenuController>();
        if (mc != null)
        {
            Report("MainMenu.SettingsPopup", mc.SettingsPopup);
            Report("MainMenu.StatsPopup", mc.StatsPopup);
        }

        EditorSceneManager.OpenScene(GAME_SCENE_PATH, OpenSceneMode.Single);
        GameManager gm = Object.FindObjectOfType<GameManager>();
        if (gm != null)
        {
            Report("Game.SettingsPopup", gm.SettingsPopup);
            Report("Game.SettingsButton", gm.SettingsButton);
            Report("Game.StartCountdown", gm.StartCountdown);
        }
        Debug.Log("Verification complete.");
    }

    private static void Report(string name, Object obj)
    {
        if (obj == null) Debug.LogWarning("MISSING: " + name);
        else Debug.Log("OK: " + name + " = " + obj.name);
    }

    private static SettingsPopup BuildOrFindSettingsPopup(RectTransform canvasRt)
    {
        Transform existing = canvasRt.Find("SettingsPopup");
        if (existing != null)
        {
            SettingsPopup ex = existing.GetComponent<SettingsPopup>();
            if (ex != null) return ex;
        }

        GameObject popupGo = new GameObject("SettingsPopup");
        popupGo.transform.SetParent(canvasRt, false);
        RectTransform popupRt = popupGo.AddComponent<RectTransform>();
        popupRt.anchorMin = Vector2.zero;
        popupRt.anchorMax = Vector2.one;
        popupRt.offsetMin = Vector2.zero;
        popupRt.offsetMax = Vector2.zero;

        Image backdrop = BuildBackdrop(popupRt);
        RectTransform contentRt = BuildContentCard(popupRt, 800f, 1000f);

        TextMeshProUGUI title = BuildTitle(contentRt, "SETTINGS", -80f);

        Toggle soundToggle = BuildToggleRow(contentRt, "SoundToggle", "SOUND", 120f);
        Toggle musicToggle = BuildToggleRow(contentRt, "MusicToggle", "MUSIC", -30f);
        Toggle hapticToggle = BuildToggleRow(contentRt, "HapticToggle", "VIBRATION", -180f);

        Button closeBtn = BuildPopupButton(contentRt, "CloseButton", "CLOSE", new Color(0.29f, 0.56f, 0.89f, 1f), new Vector2(0f, -380f));

        PopupBase popupBase = popupGo.AddComponent<PopupBase>();
        popupBase.Content = contentRt;
        popupBase.Backdrop = backdrop;

        SettingsPopup popup = popupGo.AddComponent<SettingsPopup>();
        popup.Popup = popupBase;
        popup.SoundToggle = soundToggle;
        popup.MusicToggle = musicToggle;
        popup.HapticToggle = hapticToggle;
        popup.CloseButton = closeBtn;

        popupGo.SetActive(false);
        return popup;
    }

    private static StatsPopup BuildOrFindStatsPopup(RectTransform canvasRt)
    {
        Transform existing = canvasRt.Find("StatsPopup");
        if (existing != null)
        {
            StatsPopup ex = existing.GetComponent<StatsPopup>();
            if (ex != null) return ex;
        }

        GameObject popupGo = new GameObject("StatsPopup");
        popupGo.transform.SetParent(canvasRt, false);
        RectTransform popupRt = popupGo.AddComponent<RectTransform>();
        popupRt.anchorMin = Vector2.zero;
        popupRt.anchorMax = Vector2.one;
        popupRt.offsetMin = Vector2.zero;
        popupRt.offsetMax = Vector2.zero;

        Image backdrop = BuildBackdrop(popupRt);
        RectTransform contentRt = BuildContentCard(popupRt, 800f, 1000f);

        TextMeshProUGUI title = BuildTitle(contentRt, "STATISTICS", -80f);

        TextMeshProUGUI bestScoreText = BuildStatRow(contentRt, "BestScoreText", "BEST SCORE  0", 100f);
        TextMeshProUGUI totalGamesText = BuildStatRow(contentRt, "TotalGamesText", "GAMES PLAYED  0", -20f);
        TextMeshProUGUI totalBlocksText = BuildStatRow(contentRt, "TotalBlocksText", "BLOCKS BROKEN  0", -140f);
        TextMeshProUGUI bestComboText = BuildStatRow(contentRt, "BestComboText", "BEST COMBO  0", -260f);

        Button closeBtn = BuildPopupButton(contentRt, "CloseButton", "CLOSE", new Color(0.29f, 0.56f, 0.89f, 1f), new Vector2(0f, -400f));

        PopupBase popupBase = popupGo.AddComponent<PopupBase>();
        popupBase.Content = contentRt;
        popupBase.Backdrop = backdrop;

        StatsPopup popup = popupGo.AddComponent<StatsPopup>();
        popup.Popup = popupBase;
        popup.BestScoreText = bestScoreText;
        popup.TotalGamesText = totalGamesText;
        popup.TotalBlocksText = totalBlocksText;
        popup.BestComboText = bestComboText;
        popup.CloseButton = closeBtn;

        popupGo.SetActive(false);
        return popup;
    }

    private static Button BuildOrFindTopBarSettingsButton(Transform topBar)
    {
        Transform existing = topBar.Find("SettingsIconButton");
        if (existing != null)
        {
            Button ex = existing.GetComponent<Button>();
            if (ex != null) return ex;
        }

        GameObject go = new GameObject("SettingsIconButton");
        go.transform.SetParent(topBar, false);
        RectTransform rt = go.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(1f, 1f);
        rt.anchorMax = new Vector2(1f, 1f);
        rt.pivot = new Vector2(1f, 1f);
        rt.anchoredPosition = new Vector2(-20f, -20f);
        rt.sizeDelta = new Vector2(100f, 100f);

        Image img = go.AddComponent<Image>();
        img.color = new Color(1f, 1f, 1f, 0.15f);
        img.raycastTarget = true;

        Button btn = go.AddComponent<Button>();
        btn.targetGraphic = img;
        go.AddComponent<ButtonAnimator>();

        GameObject textGo = new GameObject("Label");
        textGo.transform.SetParent(go.transform, false);
        RectTransform textRt = textGo.AddComponent<RectTransform>();
        textRt.anchorMin = Vector2.zero;
        textRt.anchorMax = Vector2.one;
        textRt.offsetMin = Vector2.zero;
        textRt.offsetMax = Vector2.zero;
        TextMeshProUGUI tmp = textGo.AddComponent<TextMeshProUGUI>();
        tmp.text = "\u2699";
        tmp.fontSize = 56f;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.white;
        tmp.raycastTarget = false;

        return btn;
    }

    private static StartCountdown BuildOrFindCountdown(RectTransform canvasRt)
    {
        Transform existing = canvasRt.Find("StartCountdown");
        if (existing != null)
        {
            StartCountdown ex = existing.GetComponent<StartCountdown>();
            if (ex != null) return ex;
        }

        GameObject go = new GameObject("StartCountdown");
        go.transform.SetParent(canvasRt, false);
        RectTransform rt = go.AddComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        CanvasGroup cg = go.AddComponent<CanvasGroup>();

        Image dim = go.AddComponent<Image>();
        dim.color = new Color(0f, 0f, 0f, 0.35f);
        dim.raycastTarget = true;

        GameObject textGo = new GameObject("CountdownText");
        textGo.transform.SetParent(go.transform, false);
        RectTransform textRt = textGo.AddComponent<RectTransform>();
        textRt.anchorMin = new Vector2(0.5f, 0.5f);
        textRt.anchorMax = new Vector2(0.5f, 0.5f);
        textRt.pivot = new Vector2(0.5f, 0.5f);
        textRt.anchoredPosition = Vector2.zero;
        textRt.sizeDelta = new Vector2(500f, 400f);
        TextMeshProUGUI tmp = textGo.AddComponent<TextMeshProUGUI>();
        tmp.text = "3";
        tmp.fontSize = 220f;
        tmp.fontStyle = FontStyles.Bold;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.white;
        tmp.raycastTarget = false;

        StartCountdown countdown = go.AddComponent<StartCountdown>();
        countdown.CountdownText = tmp;
        countdown.CanvasGroup = cg;

        go.transform.SetAsLastSibling();

        return countdown;
    }

    private static Image BuildBackdrop(RectTransform parent)
    {
        GameObject backdropGo = new GameObject("Backdrop");
        backdropGo.transform.SetParent(parent, false);
        RectTransform bRt = backdropGo.AddComponent<RectTransform>();
        bRt.anchorMin = Vector2.zero;
        bRt.anchorMax = Vector2.one;
        bRt.offsetMin = Vector2.zero;
        bRt.offsetMax = Vector2.zero;
        Image backdrop = backdropGo.AddComponent<Image>();
        backdrop.color = new Color(0f, 0f, 0f, 0f);
        backdrop.raycastTarget = true;
        return backdrop;
    }

    private static RectTransform BuildContentCard(RectTransform parent, float width, float height)
    {
        GameObject contentGo = new GameObject("Content");
        contentGo.transform.SetParent(parent, false);
        RectTransform contentRt = contentGo.AddComponent<RectTransform>();
        contentRt.anchorMin = new Vector2(0.5f, 0.5f);
        contentRt.anchorMax = new Vector2(0.5f, 0.5f);
        contentRt.pivot = new Vector2(0.5f, 0.5f);
        contentRt.anchoredPosition = Vector2.zero;
        contentRt.sizeDelta = new Vector2(width, height);
        Image contentBg = contentGo.AddComponent<Image>();
        contentBg.color = new Color(0.17f, 0.17f, 0.28f, 1f);
        contentBg.raycastTarget = true;
        return contentRt;
    }

    private static TextMeshProUGUI BuildTitle(RectTransform parent, string text, float yOffset)
    {
        GameObject titleGo = new GameObject("Title");
        titleGo.transform.SetParent(parent, false);
        RectTransform titleRt = titleGo.AddComponent<RectTransform>();
        titleRt.anchorMin = new Vector2(0.5f, 1f);
        titleRt.anchorMax = new Vector2(0.5f, 1f);
        titleRt.pivot = new Vector2(0.5f, 1f);
        titleRt.anchoredPosition = new Vector2(0f, yOffset);
        titleRt.sizeDelta = new Vector2(700f, 130f);
        TextMeshProUGUI tmp = titleGo.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = 72f;
        tmp.fontStyle = FontStyles.Bold;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.white;
        tmp.raycastTarget = false;
        return tmp;
    }

    private static Toggle BuildToggleRow(RectTransform parent, string name, string label, float yOffset)
    {
        GameObject rowGo = new GameObject(name + "Row");
        rowGo.transform.SetParent(parent, false);
        RectTransform rowRt = rowGo.AddComponent<RectTransform>();
        rowRt.anchorMin = new Vector2(0.5f, 0.5f);
        rowRt.anchorMax = new Vector2(0.5f, 0.5f);
        rowRt.pivot = new Vector2(0.5f, 0.5f);
        rowRt.anchoredPosition = new Vector2(0f, yOffset);
        rowRt.sizeDelta = new Vector2(650f, 100f);

        GameObject labelGo = new GameObject("Label");
        labelGo.transform.SetParent(rowRt, false);
        RectTransform labelRt = labelGo.AddComponent<RectTransform>();
        labelRt.anchorMin = new Vector2(0f, 0.5f);
        labelRt.anchorMax = new Vector2(0f, 0.5f);
        labelRt.pivot = new Vector2(0f, 0.5f);
        labelRt.anchoredPosition = Vector2.zero;
        labelRt.sizeDelta = new Vector2(400f, 90f);
        TextMeshProUGUI labelTmp = labelGo.AddComponent<TextMeshProUGUI>();
        labelTmp.text = label;
        labelTmp.fontSize = 48f;
        labelTmp.fontStyle = FontStyles.Bold;
        labelTmp.alignment = TextAlignmentOptions.Left;
        labelTmp.color = Color.white;
        labelTmp.raycastTarget = false;

        GameObject toggleGo = new GameObject(name);
        toggleGo.transform.SetParent(rowRt, false);
        RectTransform toggleRt = toggleGo.AddComponent<RectTransform>();
        toggleRt.anchorMin = new Vector2(1f, 0.5f);
        toggleRt.anchorMax = new Vector2(1f, 0.5f);
        toggleRt.pivot = new Vector2(1f, 0.5f);
        toggleRt.anchoredPosition = Vector2.zero;
        toggleRt.sizeDelta = new Vector2(120f, 64f);

        Image bgImg = toggleGo.AddComponent<Image>();
        bgImg.color = new Color(1f, 1f, 1f, 0.15f);

        GameObject checkGo = new GameObject("Checkmark");
        checkGo.transform.SetParent(toggleRt, false);
        RectTransform checkRt = checkGo.AddComponent<RectTransform>();
        checkRt.anchorMin = new Vector2(0f, 0f);
        checkRt.anchorMax = new Vector2(0.5f, 1f);
        checkRt.offsetMin = new Vector2(6f, 6f);
        checkRt.offsetMax = new Vector2(-6f, -6f);
        Image checkImg = checkGo.AddComponent<Image>();
        checkImg.color = new Color(0.29f, 0.56f, 0.89f, 1f);

        Toggle toggle = toggleGo.AddComponent<Toggle>();
        toggle.targetGraphic = bgImg;
        toggle.graphic = checkImg;
        toggle.isOn = true;

        return toggle;
    }

    private static TextMeshProUGUI BuildStatRow(RectTransform parent, string name, string text, float yOffset)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(parent, false);
        RectTransform rt = go.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = new Vector2(0f, yOffset);
        rt.sizeDelta = new Vector2(700f, 90f);
        TextMeshProUGUI tmp = go.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = 46f;
        tmp.fontStyle = FontStyles.Bold;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.white;
        tmp.raycastTarget = false;
        return tmp;
    }

    private static Button BuildPopupButton(RectTransform parent, string name, string label, Color color, Vector2 anchoredPos)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(parent, false);
        RectTransform rt = go.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = anchoredPos;
        rt.sizeDelta = new Vector2(500f, 130f);

        Image img = go.AddComponent<Image>();
        img.color = color;
        img.raycastTarget = true;

        Button btn = go.AddComponent<Button>();
        btn.targetGraphic = img;
        go.AddComponent<ButtonAnimator>();

        GameObject textGo = new GameObject("Label");
        textGo.transform.SetParent(go.transform, false);
        RectTransform trt = textGo.AddComponent<RectTransform>();
        trt.anchorMin = Vector2.zero;
        trt.anchorMax = Vector2.one;
        trt.offsetMin = Vector2.zero;
        trt.offsetMax = Vector2.zero;
        TextMeshProUGUI tmp = textGo.AddComponent<TextMeshProUGUI>();
        tmp.text = label;
        tmp.fontSize = 56f;
        tmp.fontStyle = FontStyles.Bold;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.white;
        tmp.raycastTarget = false;

        return btn;
    }
}
