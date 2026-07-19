using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GridManager Grid;
    public PlayerCube Player;
    public DragDetector DragDetector;
    public MatchDetector MatchDetector;
    public SearchTimer SearchTimer;
    public ScoreManager ScoreManager;
    public RowBreaker RowBreaker;
    public HUDController HUD;
    public GameOverPopup GameOverPopup;
    public ComboManager ComboManager;
    public PlayerColorSwitcher ColorSwitcher;
    public ScreenShaker ScreenShaker;
    public ComboText ComboText;
    public StartCountdown StartCountdown;
    public SettingsPopup SettingsPopup;
    public UnityEngine.UI.Button SettingsButton;
    public ConfettiBurst Confetti;
    public ExitConfirmPopup ExitConfirmPopup;
    public UnityEngine.UI.Button ExitButton;
    public TutorialPopup TutorialPopup;

    public GameState State { get; private set; } = GameState.Playing;

    private float dragStartPointerX;
    private float dragStartCubeX;

    private void Start()
    {
        Debug.Assert(Grid != null, "Grid not assigned!");
        Debug.Assert(Player != null, "Player not assigned!");
        Debug.Assert(DragDetector != null, "DragDetector not assigned!");
        Debug.Assert(MatchDetector != null, "MatchDetector not assigned!");
        Debug.Assert(SearchTimer != null, "SearchTimer not assigned!");
        Debug.Assert(ScoreManager != null, "ScoreManager not assigned!");
        Debug.Assert(RowBreaker != null, "RowBreaker not assigned!");
        Debug.Assert(HUD != null, "HUD not assigned!");
        Debug.Assert(GameOverPopup != null, "GameOverPopup not assigned!");
        Debug.Assert(ComboManager != null, "ComboManager not assigned!");
        Debug.Assert(ColorSwitcher != null, "ColorSwitcher not assigned!");
        Debug.Assert(ScreenShaker != null, "ScreenShaker not assigned!");
        Debug.Assert(ComboText != null, "ComboText not assigned!");
        Debug.Assert(StartCountdown != null, "StartCountdown not assigned!");
        Debug.Assert(SettingsPopup != null, "SettingsPopup not assigned!");
        Debug.Assert(SettingsButton != null, "SettingsButton not assigned!");
        Debug.Assert(Confetti != null, "Confetti not assigned!");
        Debug.Assert(ExitConfirmPopup != null, "ExitConfirmPopup not assigned!");
        Debug.Assert(ExitButton != null, "ExitButton not assigned!");
        Debug.Assert(TutorialPopup != null, "TutorialPopup not assigned!");

        SettingsButton.onClick.RemoveListener(HandleSettingsClicked);
        SettingsButton.onClick.AddListener(HandleSettingsClicked);

        ExitButton.onClick.RemoveListener(HandleExitClicked);
        ExitButton.onClick.AddListener(HandleExitClicked);

        ExitConfirmPopup.OnConfirmExit -= HandleExitConfirmed;
        ExitConfirmPopup.OnConfirmExit += HandleExitConfirmed;

        Player.SetColor(0);
        Player.SetColumnInstant(2, Grid.CellSize, Grid.Columns);

        DragDetector.transform.SetSiblingIndex(1);

        SubscribeEvents();

        HUD.Initialize();
        ScoreManager.ResetScore();
        HUD.SetScoreImmediate(0);
        ComboManager.ResetCombo();
        ColorSwitcher.Initialize();
        StatsManager.RegisterGamePlayed();
        Grid.StartGrid();
        State = GameState.Playing;

        StartCountdown.OnCountdownFinished -= HandleCountdownFinished;
        StartCountdown.OnCountdownFinished += HandleCountdownFinished;

        TutorialPopup.OnTutorialFinished -= HandleTutorialFinished;
        TutorialPopup.OnTutorialFinished += HandleTutorialFinished;

        if (TutorialPopup.ShouldShow())
        {
            TutorialPopup.Show();
        }
        else
        {
            StartCountdown.PlayCountdown();
        }
    }

    private void HandleTutorialFinished()
    {
        StartCountdown.PlayCountdown();
    }

    private void HandleCountdownFinished()
    {
        MatchDetector.StartDetection();
        SearchTimer.StartTimer();
    }

    private void HandleSettingsClicked()
    {
        if (SoundManager.Instance != null) SoundManager.Instance.PlayButton();
        SettingsPopup.Show();
    }

    private void HandleExitClicked()
    {
        if (State != GameState.Playing) return;
        if (SoundManager.Instance != null) SoundManager.Instance.PlayButton();
        ExitConfirmPopup.Show();
    }

    private void HandleExitConfirmed()
    {
        TransitionManager.Instance.LoadScene("MainMenu");
    }

    private void SubscribeEvents()
    {
        DragDetector.OnDragBegin -= HandleDragBegin;
        DragDetector.OnDragBegin += HandleDragBegin;
        DragDetector.OnDragMove -= HandleDragMove;
        DragDetector.OnDragMove += HandleDragMove;
        DragDetector.OnDragEnd -= HandleDragEnd;
        DragDetector.OnDragEnd += HandleDragEnd;

        MatchDetector.OnMatch -= HandleMatch;
        MatchDetector.OnMatch += HandleMatch;

        SearchTimer.OnTimeChanged -= HandleTimerChanged;
        SearchTimer.OnTimeChanged += HandleTimerChanged;
        SearchTimer.OnTimeout -= HandleTimeout;
        SearchTimer.OnTimeout += HandleTimeout;

        ScoreManager.OnScoreChanged -= HandleScoreChanged;
        ScoreManager.OnScoreChanged += HandleScoreChanged;

        GameOverPopup.OnRestart -= HandleRestart;
        GameOverPopup.OnRestart += HandleRestart;
        GameOverPopup.OnMenu -= HandleMenu;
        GameOverPopup.OnMenu += HandleMenu;

        ComboManager.OnComboMilestone -= HandleComboMilestone;
        ComboManager.OnComboMilestone += HandleComboMilestone;
    }

    private void UnsubscribeEvents()
    {
        if (DragDetector != null)
        {
            DragDetector.OnDragBegin -= HandleDragBegin;
            DragDetector.OnDragMove -= HandleDragMove;
            DragDetector.OnDragEnd -= HandleDragEnd;
        }
        if (MatchDetector != null) MatchDetector.OnMatch -= HandleMatch;
        if (SearchTimer != null)
        {
            SearchTimer.OnTimeChanged -= HandleTimerChanged;
            SearchTimer.OnTimeout -= HandleTimeout;
        }
        if (ScoreManager != null) ScoreManager.OnScoreChanged -= HandleScoreChanged;
        if (GameOverPopup != null)
        {
            GameOverPopup.OnRestart -= HandleRestart;
            GameOverPopup.OnMenu -= HandleMenu;
        }
        if (ComboManager != null) ComboManager.OnComboMilestone -= HandleComboMilestone;
        if (StartCountdown != null) StartCountdown.OnCountdownFinished -= HandleCountdownFinished;
        if (ExitConfirmPopup != null) ExitConfirmPopup.OnConfirmExit -= HandleExitConfirmed;
        if (TutorialPopup != null) TutorialPopup.OnTutorialFinished -= HandleTutorialFinished;
    }

    private void HandleDragBegin(float pointerCanvasX)
    {
        if (State != GameState.Playing) return;
        dragStartPointerX = pointerCanvasX;
        dragStartCubeX = Player.Rect.anchoredPosition.x;
        if (SoundManager.Instance != null) SoundManager.Instance.PlayTap();
    }

    private void HandleDragMove(float pointerCanvasX)
    {
        if (State != GameState.Playing) return;
        float delta = pointerCanvasX - dragStartPointerX;
        float targetX = dragStartCubeX + delta;

        float halfRange = (Grid.Columns - 1) * Grid.CellSize * 0.5f;
        targetX = Mathf.Clamp(targetX, -halfRange, halfRange);

        Player.SetXInstant(targetX);
    }

    private void HandleDragEnd()
    {
        if (State != GameState.Playing) return;
        Player.SnapToNearestColumn(Grid.CellSize, Grid.Columns);
    }

    private void HandleMatch(RowContainer row, int column)
    {
        if (State != GameState.Playing) return;

        RowBreaker.BreakRow(row, column);
        Grid.OnRowMatched(row);
        StartCoroutine(ReturnRowDelayed(row, 0.3f));

        MatchDetector.StopDetection();
        StartCoroutine(ResumeDetectionDelayed(Grid.LastAdvanceDuration));

        ScoreManager.AddScore(1);
        SearchTimer.ResetOnMatch();
        Player.PunchSuccess();
        ComboManager.RegisterMatch();
        ColorSwitcher.RegisterRowCleared();
        Grid.EnsureColorInTopRow(Player.ColorIndex);
        StatsManager.RegisterBlockBroken();

        ScreenShaker.ShakeSmall();

        if (SoundManager.Instance != null) SoundManager.Instance.PlayMatch();
        if (HapticManager.Instance != null) HapticManager.Instance.Light();
    }

    private void HandleComboMilestone(string label)
    {
        ComboText.Show(label);
        Confetti.Burst(Player.Rect.anchoredPosition);

        if (label == "GREAT!")
        {
            ScreenShaker.ShakeMedium();
            if (SoundManager.Instance != null) SoundManager.Instance.PlayComboGreat();
            if (HapticManager.Instance != null) HapticManager.Instance.Medium();
        }
        else
        {
            ScreenShaker.ShakeBig();
            if (SoundManager.Instance != null) SoundManager.Instance.PlayComboAmazing();
            if (HapticManager.Instance != null) HapticManager.Instance.Heavy();
        }
    }

    private System.Collections.IEnumerator ReturnRowDelayed(RowContainer row, float delay)
    {
        yield return new WaitForSeconds(delay);
        Grid.ReturnRowToPool(row);
    }

    private System.Collections.IEnumerator ResumeDetectionDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (State == GameState.Playing) MatchDetector.StartDetection();
    }

    private void HandleTimerChanged(float current, float max)
    {
        HUD.UpdateTimer(current, max);
    }

    private void HandleTimeout()
    {
        TriggerGameOver();
    }

    private void HandleScoreChanged(int newScore)
    {
        HUD.SetScoreAnimated(newScore);
    }

    private void TriggerGameOver()
    {
        State = GameState.GameOver;
        SearchTimer.StopTimer();
        MatchDetector.StopDetection();
        StatsManager.RegisterComboIfBest(ComboManager.CurrentCombo);
        ComboManager.BreakCombo();

        if (SoundManager.Instance != null) SoundManager.Instance.PlayGameOver();
        if (HapticManager.Instance != null) HapticManager.Instance.Medium();

        GameOverPopup.Show(ScoreManager.Score, ScoreManager.BestScore);
    }

    private void HandleRestart()
    {
        TransitionManager.Instance.LoadScene("Game");
    }

    private void HandleMenu()
    {
        TransitionManager.Instance.LoadScene("MainMenu");
    }

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }
}
