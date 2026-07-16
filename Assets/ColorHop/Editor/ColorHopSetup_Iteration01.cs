using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class ColorHopSetup_Iteration01
{
    private const string MENU_ROOT = "ColorHop/Setup (Iteration 1)/";
    private const string SCENES_FOLDER = "Assets/ColorHop/Scenes";
    private const string MAIN_MENU_SCENE_PATH = SCENES_FOLDER + "/MainMenu.unity";
    private const string GAME_SCENE_PATH = SCENES_FOLDER + "/Game.unity";

    [MenuItem(MENU_ROOT + "1. Create MainMenu Scene")]
    public static void CreateMainMenuScene()
    {
        if (!AskOverwriteIfExists(MAIN_MENU_SCENE_PATH, "MainMenu")) return;

        EnsureScenesFolder();

        UnityEngine.SceneManagement.Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        CreateCamera(ColorPalette.Background);
        CreateEventSystem();
        CreateGameBootstrap();

        GameObject canvasGo = CreateCanvas("Canvas");
        RectTransform canvasRt = canvasGo.GetComponent<RectTransform>();

        CreateBackgroundImage(canvasRt);

        GameObject safeAreaGo = CreateSafeAreaPanel(canvasRt);
        RectTransform safeAreaRt = safeAreaGo.GetComponent<RectTransform>();

        CreateTitle(safeAreaRt);

        Button playBtn = CreateBigButton(safeAreaRt, "PlayButton", "PLAY", ColorPalette.Primary, new Vector2(0f, 100f), new Vector2(600f, 200f));
        Button settingsBtn = CreateIconButton(safeAreaRt, "SettingsButton", "SETTINGS", new Vector2(-180f, -180f));
        Button statsBtn = CreateIconButton(safeAreaRt, "StatsButton", "STATS", new Vector2(180f, -180f));

        GameObject controllerGo = new GameObject("MainMenuController");
        controllerGo.transform.SetParent(canvasGo.transform, false);
        MainMenuController controller = controllerGo.AddComponent<MainMenuController>();
        controller.PlayButton = playBtn;
        controller.SettingsButton = settingsBtn;
        controller.StatsButton = statsBtn;

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene, MAIN_MENU_SCENE_PATH);
        Debug.Log("MainMenu scene created at " + MAIN_MENU_SCENE_PATH);
    }

    [MenuItem(MENU_ROOT + "2. Create Game Scene")]
    public static void CreateGameScene()
    {
        if (!AskOverwriteIfExists(GAME_SCENE_PATH, "Game")) return;

        EnsureScenesFolder();

        UnityEngine.SceneManagement.Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        CreateCamera(ColorPalette.Background);
        CreateEventSystem();
        CreateGameBootstrap();

        GameObject canvasGo = CreateCanvas("Canvas");
        RectTransform canvasRt = canvasGo.GetComponent<RectTransform>();

        CreateBackgroundImage(canvasRt);

        GameObject gridRootGo = new GameObject("GridRoot");
        gridRootGo.transform.SetParent(canvasRt, false);
        RectTransform gridRootRt = gridRootGo.AddComponent<RectTransform>();
        gridRootRt.anchorMin = new Vector2(0.5f, 0.5f);
        gridRootRt.anchorMax = new Vector2(0.5f, 0.5f);
        gridRootRt.pivot = new Vector2(0.5f, 0.5f);
        gridRootRt.anchoredPosition = Vector2.zero;
        gridRootRt.sizeDelta = new Vector2(1080f, 1920f);

        GameObject safeAreaGo = CreateSafeAreaPanel(canvasRt);
        RectTransform safeAreaRt = safeAreaGo.GetComponent<RectTransform>();

        GameObject topBarGo = new GameObject("TopBar");
        topBarGo.transform.SetParent(safeAreaRt, false);
        RectTransform topBarRt = topBarGo.AddComponent<RectTransform>();
        topBarRt.anchorMin = new Vector2(0f, 1f);
        topBarRt.anchorMax = new Vector2(1f, 1f);
        topBarRt.pivot = new Vector2(0.5f, 1f);
        topBarRt.anchoredPosition = new Vector2(0f, 0f);
        topBarRt.sizeDelta = new Vector2(0f, 180f);

        GameObject playerGo = new GameObject("PlayerCube");
        playerGo.transform.SetParent(canvasRt, false);
        RectTransform playerRt = playerGo.AddComponent<RectTransform>();
        playerRt.anchorMin = new Vector2(0.5f, 0.5f);
        playerRt.anchorMax = new Vector2(0.5f, 0.5f);
        playerRt.pivot = new Vector2(0.5f, 0.5f);
        playerRt.anchoredPosition = new Vector2(0f, 550f);
        playerRt.sizeDelta = new Vector2(200f, 200f);
        Image playerImg = playerGo.AddComponent<Image>();
        playerImg.color = ColorPalette.Colors[0];
        playerImg.raycastTarget = false;
        PlayerCube player = playerGo.AddComponent<PlayerCube>();
        player.Image = playerImg;
        player.Rect = playerRt;

        GameObject swipeAreaGo = new GameObject("SwipeArea");
        swipeAreaGo.transform.SetParent(canvasRt, false);
        RectTransform swipeAreaRt = swipeAreaGo.AddComponent<RectTransform>();
        swipeAreaRt.anchorMin = Vector2.zero;
        swipeAreaRt.anchorMax = Vector2.one;
        swipeAreaRt.offsetMin = Vector2.zero;
        swipeAreaRt.offsetMax = Vector2.zero;
        Image swipeImg = swipeAreaGo.AddComponent<Image>();
        swipeImg.color = new Color(0f, 0f, 0f, 0f);
        swipeImg.raycastTarget = true;
        DragDetector dragDetector = swipeAreaGo.AddComponent<DragDetector>();
        dragDetector.CanvasRect = canvasRt;

        GameObject managersGo = new GameObject("GameManager");
        managersGo.transform.SetParent(canvasRt, false);
        GridManager grid = managersGo.AddComponent<GridManager>();
        grid.GridRoot = gridRootRt;
        grid.Player = player;
        grid.PlayerY = 550f;
        GameManager gm = managersGo.AddComponent<GameManager>();
        gm.Grid = grid;
        gm.Player = player;
        gm.DragDetector = dragDetector;

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene, GAME_SCENE_PATH);
        Debug.Log("Game scene created at " + GAME_SCENE_PATH);
    }

    [MenuItem(MENU_ROOT + "3. Add Scenes to Build Settings")]
    public static void AddScenesToBuildSettings()
    {
        EditorBuildSettingsScene[] scenes = new EditorBuildSettingsScene[]
        {
            new EditorBuildSettingsScene(MAIN_MENU_SCENE_PATH, true),
            new EditorBuildSettingsScene(GAME_SCENE_PATH, true)
        };
        EditorBuildSettings.scenes = scenes;
        Debug.Log("Scenes added to Build Settings: MainMenu (0), Game (1)");
    }

    private static bool AskOverwriteIfExists(string path, string name)
    {
        if (System.IO.File.Exists(path))
        {
            bool ok = EditorUtility.DisplayDialog(
                "Scene exists",
                name + " scene already exists at " + path + ". Overwrite?",
                "Overwrite", "Cancel");
            return ok;
        }
        return true;
    }

    private static void EnsureScenesFolder()
    {
        if (!AssetDatabase.IsValidFolder("Assets/ColorHop"))
            AssetDatabase.CreateFolder("Assets", "ColorHop");
        if (!AssetDatabase.IsValidFolder(SCENES_FOLDER))
            AssetDatabase.CreateFolder("Assets/ColorHop", "Scenes");
    }

    private static void CreateCamera(Color background)
    {
        GameObject camGo = new GameObject("Main Camera");
        Camera cam = camGo.AddComponent<Camera>();
        camGo.tag = "MainCamera";
        camGo.AddComponent<AudioListener>();
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = background;
        cam.orthographic = false;
        camGo.transform.position = new Vector3(0f, 0f, -10f);
    }

    private static void CreateEventSystem()
    {
        GameObject go = new GameObject("EventSystem");
        go.AddComponent<EventSystem>();
        go.AddComponent<StandaloneInputModule>();
    }

    private static void CreateGameBootstrap()
    {
        GameObject go = new GameObject("GameBootstrap");
        go.AddComponent<GameBootstrap>();
    }

    private static GameObject CreateCanvas(string name)
    {
        GameObject go = new GameObject(name);
        Canvas canvas = go.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 0;

        CanvasScaler scaler = go.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1080f, 1920f);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 0.5f;

        go.AddComponent<GraphicRaycaster>();
        return go;
    }

    private static void CreateBackgroundImage(RectTransform parent)
    {
        GameObject go = new GameObject("Background");
        go.transform.SetParent(parent, false);
        RectTransform rt = go.AddComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        Image img = go.AddComponent<Image>();
        img.color = ColorPalette.Background;
        img.raycastTarget = false;
    }

    private static GameObject CreateSafeAreaPanel(RectTransform parent)
    {
        GameObject go = new GameObject("SafeArea");
        go.transform.SetParent(parent, false);
        RectTransform rt = go.AddComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        go.AddComponent<SafeAreaFitter>();
        return go;
    }

    private static void CreateTitle(RectTransform parent)
    {
        GameObject go = new GameObject("Title");
        go.transform.SetParent(parent, false);
        RectTransform rt = go.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 1f);
        rt.anchorMax = new Vector2(0.5f, 1f);
        rt.pivot = new Vector2(0.5f, 1f);
        rt.anchoredPosition = new Vector2(0f, -260f);
        rt.sizeDelta = new Vector2(900f, 240f);
        TextMeshProUGUI tmp = go.AddComponent<TextMeshProUGUI>();
        tmp.text = "COLOR HOP";
        tmp.fontSize = 140f;
        tmp.fontStyle = FontStyles.Bold;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.white;
        tmp.enableWordWrapping = false;
    }

    private static Button CreateBigButton(RectTransform parent, string name, string label, Color color, Vector2 anchoredPos, Vector2 size)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(parent, false);
        RectTransform rt = go.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = anchoredPos;
        rt.sizeDelta = size;

        Image img = go.AddComponent<Image>();
        img.color = color;
        img.raycastTarget = true;

        Button btn = go.AddComponent<Button>();
        btn.targetGraphic = img;
        ColorBlock cb = btn.colors;
        cb.normalColor = Color.white;
        cb.highlightedColor = Color.white;
        cb.pressedColor = new Color(0.85f, 0.85f, 0.85f, 1f);
        cb.selectedColor = Color.white;
        cb.disabledColor = new Color(1f, 1f, 1f, 0.5f);
        btn.colors = cb;

        go.AddComponent<ButtonAnimator>();

        GameObject textGo = new GameObject("Label");
        textGo.transform.SetParent(go.transform, false);
        RectTransform textRt = textGo.AddComponent<RectTransform>();
        textRt.anchorMin = Vector2.zero;
        textRt.anchorMax = Vector2.one;
        textRt.offsetMin = Vector2.zero;
        textRt.offsetMax = Vector2.zero;
        TextMeshProUGUI tmp = textGo.AddComponent<TextMeshProUGUI>();
        tmp.text = label;
        tmp.fontSize = 90f;
        tmp.fontStyle = FontStyles.Bold;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.white;
        tmp.raycastTarget = false;

        return btn;
    }

    private static Button CreateIconButton(RectTransform parent, string name, string label, Vector2 anchoredPos)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(parent, false);
        RectTransform rt = go.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0f);
        rt.anchorMax = new Vector2(0.5f, 0f);
        rt.pivot = new Vector2(0.5f, 0f);
        rt.anchoredPosition = new Vector2(anchoredPos.x, 250f);
        rt.sizeDelta = new Vector2(150f, 150f);

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
        tmp.text = label;
        tmp.fontSize = 34f;
        tmp.fontStyle = FontStyles.Bold;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.white;
        tmp.raycastTarget = false;

        return btn;
    }
}
