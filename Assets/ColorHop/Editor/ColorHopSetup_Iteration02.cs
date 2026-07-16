using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class ColorHopSetup_Iteration02
{
    private const string MENU_ROOT = "ColorHop/Setup (Iteration 2)/";
    private const string GAME_SCENE_PATH = "Assets/ColorHop/Scenes/Game.unity";

    [MenuItem(MENU_ROOT + "1. Update Game Scene")]
    public static void UpdateGameScene()
    {
        UnityEngine.SceneManagement.Scene scene = EditorSceneManager.OpenScene(GAME_SCENE_PATH, OpenSceneMode.Single);

        GameManager gm = Object.FindObjectOfType<GameManager>();
        Debug.Assert(gm != null, "GameManager not found in Game scene. Run Iteration 1 setup first!");
        if (gm == null) return;

        GridManager grid = gm.Grid;
        PlayerCube player = gm.Player;
        DragDetector drag = gm.DragDetector;

        Canvas canvas = Object.FindObjectOfType<Canvas>();
        Debug.Assert(canvas != null, "Canvas not found!");
        RectTransform canvasRt = canvas.GetComponent<RectTransform>();

        Transform safeArea = canvas.transform.Find("SafeArea");
        Debug.Assert(safeArea != null, "SafeArea not found!");
        RectTransform safeAreaRt = safeArea.GetComponent<RectTransform>();

        Transform existingTopBar = safeArea.Find("TopBar");
        HUDController hud = null;
        if (existingTopBar != null)
        {
            hud = existingTopBar.GetComponent<HUDController>();
            if (hud == null) hud = BuildHUDInTopBar(existingTopBar.gameObject);
        }
        else
        {
            hud = BuildTopBarWithHUD(safeAreaRt);
        }

        RectTransform particleParent = FindOrCreateParticleParent(canvasRt);
        RowBreaker breaker = gm.GetComponent<RowBreaker>();
        if (breaker == null) breaker = gm.gameObject.AddComponent<RowBreaker>();
        breaker.ParticleParent = particleParent;

        ScoreManager score = gm.GetComponent<ScoreManager>();
        if (score == null) score = gm.gameObject.AddComponent<ScoreManager>();

        SearchTimer timer = gm.GetComponent<SearchTimer>();
        if (timer == null) timer = gm.gameObject.AddComponent<SearchTimer>();

        MatchDetector match = gm.GetComponent<MatchDetector>();
        if (match == null) match = gm.gameObject.AddComponent<MatchDetector>();
        match.Grid = grid;
        match.Player = player;

        GameOverPopup popup = FindOrCreateGameOverPopup(canvasRt);

        gm.MatchDetector = match;
        gm.SearchTimer = timer;
        gm.ScoreManager = score;
        gm.RowBreaker = breaker;
        gm.HUD = hud;
        gm.GameOverPopup = popup;

        EditorUtility.SetDirty(gm);
        EditorUtility.SetDirty(hud);
        EditorUtility.SetDirty(popup);
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        Debug.Log("Game scene updated for Iteration 2.");
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

        Report("Grid", gm.Grid);
        Report("Player", gm.Player);
        Report("DragDetector", gm.DragDetector);
        Report("MatchDetector", gm.MatchDetector);
        Report("SearchTimer", gm.SearchTimer);
        Report("ScoreManager", gm.ScoreManager);
        Report("RowBreaker", gm.RowBreaker);
        Report("HUD", gm.HUD);
        Report("GameOverPopup", gm.GameOverPopup);

        if (gm.HUD != null)
        {
            Report("HUD.ScoreText", gm.HUD.ScoreText);
            Report("HUD.TimerBarFill", gm.HUD.TimerBarFill);
            Report("HUD.TimerText", gm.HUD.TimerText);
        }
        if (gm.GameOverPopup != null)
        {
            Report("GameOverPopup.Popup", gm.GameOverPopup.Popup);
            Report("GameOverPopup.ScoreText", gm.GameOverPopup.ScoreText);
            Report("GameOverPopup.BestText", gm.GameOverPopup.BestText);
            Report("GameOverPopup.RestartButton", gm.GameOverPopup.RestartButton);
            Report("GameOverPopup.MenuButton", gm.GameOverPopup.MenuButton);
        }
        if (gm.RowBreaker != null) Report("RowBreaker.ParticleParent", gm.RowBreaker.ParticleParent);
        Debug.Log("Verification complete.");
    }

    private static void Report(string name, Object obj)
    {
        if (obj == null) Debug.LogWarning("MISSING: " + name);
        else Debug.Log("OK: " + name + " = " + obj.name);
    }

    private static HUDController BuildTopBarWithHUD(RectTransform safeAreaRt)
    {
        GameObject topBarGo = new GameObject("TopBar");
        topBarGo.transform.SetParent(safeAreaRt, false);
        RectTransform topBarRt = topBarGo.AddComponent<RectTransform>();
        topBarRt.anchorMin = new Vector2(0f, 1f);
        topBarRt.anchorMax = new Vector2(1f, 1f);
        topBarRt.pivot = new Vector2(0.5f, 1f);
        topBarRt.anchoredPosition = Vector2.zero;
        topBarRt.sizeDelta = new Vector2(0f, 260f);
        return BuildHUDInTopBar(topBarGo);
    }

    private static HUDController BuildHUDInTopBar(GameObject topBarGo)
    {
        RectTransform topBarRt = topBarGo.GetComponent<RectTransform>();

        HUDController hud = topBarGo.GetComponent<HUDController>();
        if (hud == null) hud = topBarGo.AddComponent<HUDController>();

        Transform existingScore = topBarGo.transform.Find("ScoreText");
        TextMeshProUGUI scoreTmp;
        if (existingScore == null)
        {
            GameObject scoreGo = new GameObject("ScoreText");
            scoreGo.transform.SetParent(topBarRt, false);
            RectTransform srt = scoreGo.AddComponent<RectTransform>();
            srt.anchorMin = new Vector2(0.5f, 1f);
            srt.anchorMax = new Vector2(0.5f, 1f);
            srt.pivot = new Vector2(0.5f, 1f);
            srt.anchoredPosition = new Vector2(0f, -20f);
            srt.sizeDelta = new Vector2(600f, 150f);
            scoreTmp = scoreGo.AddComponent<TextMeshProUGUI>();
            scoreTmp.text = "0";
            scoreTmp.fontSize = 130f;
            scoreTmp.fontStyle = FontStyles.Bold;
            scoreTmp.alignment = TextAlignmentOptions.Center;
            scoreTmp.color = Color.white;
            scoreTmp.raycastTarget = false;
        }
        else
        {
            scoreTmp = existingScore.GetComponent<TextMeshProUGUI>();
        }
        hud.ScoreText = scoreTmp;

        Transform existingBar = topBarGo.transform.Find("TimerBar");
        Image barFill;
        if (existingBar == null)
        {
            GameObject barBgGo = new GameObject("TimerBar");
            barBgGo.transform.SetParent(topBarRt, false);
            RectTransform barBgRt = barBgGo.AddComponent<RectTransform>();
            barBgRt.anchorMin = new Vector2(0.5f, 1f);
            barBgRt.anchorMax = new Vector2(0.5f, 1f);
            barBgRt.pivot = new Vector2(0.5f, 1f);
            barBgRt.anchoredPosition = new Vector2(0f, -190f);
            barBgRt.sizeDelta = new Vector2(500f, 24f);
            Image bg = barBgGo.AddComponent<Image>();
            bg.color = new Color(1f, 1f, 1f, 0.15f);
            bg.raycastTarget = false;

            GameObject fillGo = new GameObject("Fill");
            fillGo.transform.SetParent(barBgRt, false);
            RectTransform fillRt = fillGo.AddComponent<RectTransform>();
            fillRt.anchorMin = Vector2.zero;
            fillRt.anchorMax = Vector2.one;
            fillRt.offsetMin = Vector2.zero;
            fillRt.offsetMax = Vector2.zero;
            barFill = fillGo.AddComponent<Image>();
            barFill.color = new Color(0.29f, 0.56f, 0.89f, 1f);
            barFill.type = Image.Type.Filled;
            barFill.fillMethod = Image.FillMethod.Horizontal;
            barFill.fillOrigin = 0;
            barFill.fillAmount = 1f;
            barFill.raycastTarget = false;
        }
        else
        {
            barFill = existingBar.Find("Fill").GetComponent<Image>();
        }
        hud.TimerBarFill = barFill;

        Transform existingTimerText = topBarGo.transform.Find("TimerText");
        TextMeshProUGUI timerTmp;
        if (existingTimerText == null)
        {
            GameObject timerGo = new GameObject("TimerText");
            timerGo.transform.SetParent(topBarRt, false);
            RectTransform trt = timerGo.AddComponent<RectTransform>();
            trt.anchorMin = new Vector2(0.5f, 1f);
            trt.anchorMax = new Vector2(0.5f, 1f);
            trt.pivot = new Vector2(0.5f, 1f);
            trt.anchoredPosition = new Vector2(0f, -220f);
            trt.sizeDelta = new Vector2(200f, 40f);
            timerTmp = timerGo.AddComponent<TextMeshProUGUI>();
            timerTmp.text = "3.0";
            timerTmp.fontSize = 32f;
            timerTmp.fontStyle = FontStyles.Bold;
            timerTmp.alignment = TextAlignmentOptions.Center;
            timerTmp.color = new Color(1f, 1f, 1f, 0.7f);
            timerTmp.raycastTarget = false;
        }
        else
        {
            timerTmp = existingTimerText.GetComponent<TextMeshProUGUI>();
        }
        hud.TimerText = timerTmp;

        return hud;
    }

    private static RectTransform FindOrCreateParticleParent(RectTransform canvasRt)
    {
        Transform existing = canvasRt.Find("ParticleLayer");
        if (existing != null) return existing.GetComponent<RectTransform>();

        GameObject go = new GameObject("ParticleLayer");
        go.transform.SetParent(canvasRt, false);
        RectTransform rt = go.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = Vector2.zero;
        rt.sizeDelta = Vector2.zero;

        Transform playerT = canvasRt.Find("PlayerCube");
        if (playerT != null)
        {
            int playerIdx = playerT.GetSiblingIndex();
            go.transform.SetSiblingIndex(playerIdx);
        }

        return rt;
    }

    private static GameOverPopup FindOrCreateGameOverPopup(RectTransform canvasRt)
    {
        Transform existing = canvasRt.Find("GameOverPopup");
        if (existing != null)
        {
            GameOverPopup ex = existing.GetComponent<GameOverPopup>();
            if (ex != null) return ex;
        }

        GameObject popupGo = new GameObject("GameOverPopup");
        popupGo.transform.SetParent(canvasRt, false);
        RectTransform popupRt = popupGo.AddComponent<RectTransform>();
        popupRt.anchorMin = Vector2.zero;
        popupRt.anchorMax = Vector2.one;
        popupRt.offsetMin = Vector2.zero;
        popupRt.offsetMax = Vector2.zero;

        GameObject backdropGo = new GameObject("Backdrop");
        backdropGo.transform.SetParent(popupRt, false);
        RectTransform bRt = backdropGo.AddComponent<RectTransform>();
        bRt.anchorMin = Vector2.zero;
        bRt.anchorMax = Vector2.one;
        bRt.offsetMin = Vector2.zero;
        bRt.offsetMax = Vector2.zero;
        Image backdrop = backdropGo.AddComponent<Image>();
        backdrop.color = new Color(0f, 0f, 0f, 0f);
        backdrop.raycastTarget = true;

        GameObject contentGo = new GameObject("Content");
        contentGo.transform.SetParent(popupRt, false);
        RectTransform contentRt = contentGo.AddComponent<RectTransform>();
        contentRt.anchorMin = new Vector2(0.5f, 0.5f);
        contentRt.anchorMax = new Vector2(0.5f, 0.5f);
        contentRt.pivot = new Vector2(0.5f, 0.5f);
        contentRt.anchoredPosition = Vector2.zero;
        contentRt.sizeDelta = new Vector2(800f, 900f);
        Image contentBg = contentGo.AddComponent<Image>();
        contentBg.color = new Color(0.17f, 0.17f, 0.28f, 1f);
        contentBg.raycastTarget = true;

        GameObject titleGo = new GameObject("Title");
        titleGo.transform.SetParent(contentRt, false);
        RectTransform titleRt = titleGo.AddComponent<RectTransform>();
        titleRt.anchorMin = new Vector2(0.5f, 1f);
        titleRt.anchorMax = new Vector2(0.5f, 1f);
        titleRt.pivot = new Vector2(0.5f, 1f);
        titleRt.anchoredPosition = new Vector2(0f, -70f);
        titleRt.sizeDelta = new Vector2(700f, 130f);
        TextMeshProUGUI titleTmp = titleGo.AddComponent<TextMeshProUGUI>();
        titleTmp.text = "GAME OVER";
        titleTmp.fontSize = 90f;
        titleTmp.fontStyle = FontStyles.Bold;
        titleTmp.alignment = TextAlignmentOptions.Center;
        titleTmp.color = Color.white;
        titleTmp.raycastTarget = false;

        GameObject scoreGo = new GameObject("ScoreText");
        scoreGo.transform.SetParent(contentRt, false);
        RectTransform scoreRt = scoreGo.AddComponent<RectTransform>();
        scoreRt.anchorMin = new Vector2(0.5f, 0.5f);
        scoreRt.anchorMax = new Vector2(0.5f, 0.5f);
        scoreRt.pivot = new Vector2(0.5f, 0.5f);
        scoreRt.anchoredPosition = new Vector2(0f, 100f);
        scoreRt.sizeDelta = new Vector2(700f, 130f);
        TextMeshProUGUI scoreTmp = scoreGo.AddComponent<TextMeshProUGUI>();
        scoreTmp.text = "SCORE 0";
        scoreTmp.fontSize = 80f;
        scoreTmp.fontStyle = FontStyles.Bold;
        scoreTmp.alignment = TextAlignmentOptions.Center;
        scoreTmp.color = Color.white;
        scoreTmp.raycastTarget = false;

        GameObject bestGo = new GameObject("BestText");
        bestGo.transform.SetParent(contentRt, false);
        RectTransform bestRt = bestGo.AddComponent<RectTransform>();
        bestRt.anchorMin = new Vector2(0.5f, 0.5f);
        bestRt.anchorMax = new Vector2(0.5f, 0.5f);
        bestRt.pivot = new Vector2(0.5f, 0.5f);
        bestRt.anchoredPosition = new Vector2(0f, 0f);
        bestRt.sizeDelta = new Vector2(700f, 100f);
        TextMeshProUGUI bestTmp = bestGo.AddComponent<TextMeshProUGUI>();
        bestTmp.text = "BEST 0";
        bestTmp.fontSize = 60f;
        bestTmp.fontStyle = FontStyles.Bold;
        bestTmp.alignment = TextAlignmentOptions.Center;
        bestTmp.color = new Color(1f, 1f, 1f, 0.7f);
        bestTmp.raycastTarget = false;

        Button restartBtn = BuildPopupButton(contentRt, "RestartButton", "RESTART", new Color(0.29f, 0.56f, 0.89f, 1f), new Vector2(0f, -220f));
        Button menuBtn = BuildPopupButton(contentRt, "MenuButton", "MENU", new Color(1f, 1f, 1f, 0.15f), new Vector2(0f, -370f));

        PopupBase popupBase = popupGo.AddComponent<PopupBase>();
        popupBase.Content = contentRt;
        popupBase.Backdrop = backdrop;

        GameOverPopup goPopup = popupGo.AddComponent<GameOverPopup>();
        goPopup.Popup = popupBase;
        goPopup.ScoreText = scoreTmp;
        goPopup.BestText = bestTmp;
        goPopup.RestartButton = restartBtn;
        goPopup.MenuButton = menuBtn;

        popupGo.SetActive(false);
        return goPopup;
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
        rt.sizeDelta = new Vector2(600f, 140f);

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
        tmp.fontSize = 64f;
        tmp.fontStyle = FontStyles.Bold;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.white;
        tmp.raycastTarget = false;

        return btn;
    }
}
