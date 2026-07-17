using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class ColorHopSetup_Iteration06
{
    private const string MENU_ROOT = "ColorHop/Setup (Iteration 6)/";
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
        Transform safeArea = canvas.transform.Find("SafeArea");
        Transform topBar = safeArea.Find("TopBar");

        Button exitBtn = BuildOrFindTopBarExitButton(topBar);
        ExitConfirmPopup exitPopup = BuildOrFindExitConfirmPopup(canvasRt);
        TutorialPopup tutorialPopup = BuildOrFindTutorialPopup(canvasRt);

        gm.ExitButton = exitBtn;
        gm.ExitConfirmPopup = exitPopup;
        gm.TutorialPopup = tutorialPopup;

        EditorUtility.SetDirty(gm);
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        Debug.Log("Game scene updated for Iteration 6.");
    }

    [MenuItem(MENU_ROOT + "2. Verify Game Scene References")]
    public static void VerifyReferences()
    {
        EditorSceneManager.OpenScene(GAME_SCENE_PATH, OpenSceneMode.Single);
        GameManager gm = Object.FindObjectOfType<GameManager>();
        if (gm != null)
        {
            Report("ExitButton", gm.ExitButton);
            Report("ExitConfirmPopup", gm.ExitConfirmPopup);
            Report("TutorialPopup", gm.TutorialPopup);
        }
        Debug.Log("Verification complete.");
    }

    [MenuItem(MENU_ROOT + "3. Reset Tutorial Seen Flag (Editor PlayerPrefs)")]
    public static void ResetTutorialFlag()
    {
        PlayerPrefs.DeleteKey("tutorial_seen");
        PlayerPrefs.Save();
        Debug.Log("tutorial_seen flag cleared. Next Play will show the tutorial again.");
    }

    private static void Report(string name, Object obj)
    {
        if (obj == null) Debug.LogWarning("MISSING: " + name);
        else Debug.Log("OK: " + name + " = " + obj.name);
    }

    private static Button BuildOrFindTopBarExitButton(Transform topBar)
    {
        Transform existing = topBar.Find("ExitIconButton");
        if (existing != null)
        {
            Button ex = existing.GetComponent<Button>();
            if (ex != null) return ex;
        }

        GameObject go = new GameObject("ExitIconButton");
        go.transform.SetParent(topBar, false);
        RectTransform rt = go.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(0f, 1f);
        rt.anchorMax = new Vector2(0f, 1f);
        rt.pivot = new Vector2(0f, 1f);
        rt.anchoredPosition = new Vector2(20f, -20f);
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
        tmp.text = "\u2715";
        tmp.fontSize = 48f;
        tmp.fontStyle = FontStyles.Bold;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.white;
        tmp.raycastTarget = false;

        return btn;
    }

    private static ExitConfirmPopup BuildOrFindExitConfirmPopup(RectTransform canvasRt)
    {
        Transform existing = canvasRt.Find("ExitConfirmPopup");
        if (existing != null)
        {
            ExitConfirmPopup ex = existing.GetComponent<ExitConfirmPopup>();
            if (ex != null) return ex;
        }

        GameObject popupGo = new GameObject("ExitConfirmPopup");
        popupGo.transform.SetParent(canvasRt, false);
        RectTransform popupRt = popupGo.AddComponent<RectTransform>();
        popupRt.anchorMin = Vector2.zero;
        popupRt.anchorMax = Vector2.one;
        popupRt.offsetMin = Vector2.zero;
        popupRt.offsetMax = Vector2.zero;

        Image backdrop = BuildBackdrop(popupRt);
        RectTransform contentRt = BuildContentCard(popupRt, 750f, 550f);

        TextMeshProUGUI title = BuildTitle(contentRt, "LEAVE GAME?", -70f);

        GameObject bodyGo = new GameObject("Body");
        bodyGo.transform.SetParent(contentRt, false);
        RectTransform bodyRt = bodyGo.AddComponent<RectTransform>();
        bodyRt.anchorMin = new Vector2(0.5f, 0.5f);
        bodyRt.anchorMax = new Vector2(0.5f, 0.5f);
        bodyRt.pivot = new Vector2(0.5f, 0.5f);
        bodyRt.anchoredPosition = new Vector2(0f, 60f);
        bodyRt.sizeDelta = new Vector2(600f, 120f);
        TextMeshProUGUI bodyTmp = bodyGo.AddComponent<TextMeshProUGUI>();
        bodyTmp.text = "Your current run will be lost.";
        bodyTmp.fontSize = 42f;
        bodyTmp.alignment = TextAlignmentOptions.Center;
        bodyTmp.color = new Color(1f, 1f, 1f, 0.75f);
        bodyTmp.raycastTarget = false;

        Button confirmBtn = BuildPopupButton(contentRt, "ConfirmButton", "LEAVE", new Color(0.96f, 0.26f, 0.51f, 1f), new Vector2(0f, -80f));
        Button cancelBtn = BuildPopupButton(contentRt, "CancelButton", "STAY", new Color(1f, 1f, 1f, 0.15f), new Vector2(0f, -220f));

        PopupBase popupBase = popupGo.AddComponent<PopupBase>();
        popupBase.Content = contentRt;
        popupBase.Backdrop = backdrop;

        ExitConfirmPopup popup = popupGo.AddComponent<ExitConfirmPopup>();
        popup.Popup = popupBase;
        popup.ConfirmButton = confirmBtn;
        popup.CancelButton = cancelBtn;

        popupGo.SetActive(false);
        return popup;
    }

    private static TutorialPopup BuildOrFindTutorialPopup(RectTransform canvasRt)
    {
        Transform existing = canvasRt.Find("TutorialPopup");
        if (existing != null)
        {
            TutorialPopup ex = existing.GetComponent<TutorialPopup>();
            if (ex != null) return ex;
        }

        GameObject popupGo = new GameObject("TutorialPopup");
        popupGo.transform.SetParent(canvasRt, false);
        RectTransform popupRt = popupGo.AddComponent<RectTransform>();
        popupRt.anchorMin = Vector2.zero;
        popupRt.anchorMax = Vector2.one;
        popupRt.offsetMin = Vector2.zero;
        popupRt.offsetMax = Vector2.zero;

        Image backdrop = BuildBackdrop(popupRt);
        backdrop.color = new Color(0.06f, 0.06f, 0.1f, 1f);
        RectTransform contentRt = BuildContentCard(popupRt, 850f, 1000f);

        TextMeshProUGUI stepTitle = BuildTitle(contentRt, "MOVE", -120f);
        stepTitle.fontSize = 80f;

        GameObject bodyGo = new GameObject("StepBody");
        bodyGo.transform.SetParent(contentRt, false);
        RectTransform bodyRt = bodyGo.AddComponent<RectTransform>();
        bodyRt.anchorMin = new Vector2(0.5f, 0.5f);
        bodyRt.anchorMax = new Vector2(0.5f, 0.5f);
        bodyRt.pivot = new Vector2(0.5f, 0.5f);
        bodyRt.anchoredPosition = new Vector2(0f, 80f);
        bodyRt.sizeDelta = new Vector2(650f, 300f);
        TextMeshProUGUI bodyTmp = bodyGo.AddComponent<TextMeshProUGUI>();
        bodyTmp.text = "Drag anywhere on screen to slide your cube left and right.";
        bodyTmp.fontSize = 46f;
        bodyTmp.alignment = TextAlignmentOptions.Center;
        bodyTmp.color = new Color(1f, 1f, 1f, 0.85f);
        bodyTmp.raycastTarget = false;
        bodyTmp.enableWordWrapping = true;

        GameObject dotsGo = new GameObject("StepDots");
        dotsGo.transform.SetParent(contentRt, false);
        RectTransform dotsRt = dotsGo.AddComponent<RectTransform>();
        dotsRt.anchorMin = new Vector2(0.5f, 0.5f);
        dotsRt.anchorMax = new Vector2(0.5f, 0.5f);
        dotsRt.pivot = new Vector2(0.5f, 0.5f);
        dotsRt.anchoredPosition = new Vector2(0f, -220f);
        dotsRt.sizeDelta = new Vector2(200f, 40f);

        System.Collections.Generic.List<Image> dots = new System.Collections.Generic.List<Image>();
        for (int i = 0; i < 3; i++)
        {
            GameObject dotGo = new GameObject("Dot" + i);
            dotGo.transform.SetParent(dotsRt, false);
            RectTransform dotRt = dotGo.AddComponent<RectTransform>();
            dotRt.anchorMin = new Vector2(0.5f, 0.5f);
            dotRt.anchorMax = new Vector2(0.5f, 0.5f);
            dotRt.pivot = new Vector2(0.5f, 0.5f);
            dotRt.sizeDelta = new Vector2(20f, 20f);
            dotRt.anchoredPosition = new Vector2((i - 1) * 50f, 0f);
            Image dotImg = dotGo.AddComponent<Image>();
            dotImg.color = i == 0 ? Color.white : new Color(1f, 1f, 1f, 0.3f);
            dots.Add(dotImg);
        }

        Button nextBtn = BuildPopupButton(contentRt, "NextButton", "NEXT", new Color(0.29f, 0.56f, 0.89f, 1f), new Vector2(0f, -350f));
        TextMeshProUGUI nextLabel = nextBtn.GetComponentInChildren<TextMeshProUGUI>();

        PopupBase popupBase = popupGo.AddComponent<PopupBase>();
        popupBase.Content = contentRt;
        popupBase.Backdrop = backdrop;

        TutorialPopup tutorial = popupGo.AddComponent<TutorialPopup>();
        tutorial.Popup = popupBase;
        tutorial.StepTitle = stepTitle;
        tutorial.StepBody = bodyTmp;
        tutorial.NextButton = nextBtn;
        tutorial.NextButtonLabel = nextLabel;
        tutorial.StepDots = dots;

        popupGo.transform.SetAsLastSibling();
        popupGo.SetActive(false);
        return tutorial;
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
