using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPopup : MonoBehaviour
{
    private const string PREF_TUTORIAL_SEEN = "tutorial_seen";

    public PopupBase Popup;
    public TextMeshProUGUI StepTitle;
    public TextMeshProUGUI StepBody;
    public Button NextButton;
    public TextMeshProUGUI NextButtonLabel;
    public List<Image> StepDots;

    public event Action OnTutorialFinished;

    private string[] titles = new string[]
    {
        "MOVE",
        "MATCH",
        "SURVIVE"
    };

    private string[] bodies = new string[]
    {
        "Drag anywhere on screen to slide your cube left and right.",
        "Stop on a block that matches your color to break the row and score.",
        "Find your color before time runs out. It gets faster the more you score!"
    };

    private int currentStep;

    private void Awake()
    {
        Debug.Assert(Popup != null, "Popup not assigned!");
        Debug.Assert(StepTitle != null, "StepTitle not assigned!");
        Debug.Assert(StepBody != null, "StepBody not assigned!");
        Debug.Assert(NextButton != null, "NextButton not assigned!");
        Debug.Assert(NextButtonLabel != null, "NextButtonLabel not assigned!");

        NextButton.onClick.AddListener(HandleNext);
    }

    public static bool ShouldShow()
    {
        return PlayerPrefs.GetInt(PREF_TUTORIAL_SEEN, 0) == 0;
    }

    public void Show()
    {
        currentStep = 0;
        RenderStep();
        Popup.Open();
    }

    private void HandleNext()
    {
        if (SoundManager.Instance != null) SoundManager.Instance.PlayButton();

        currentStep++;
        if (currentStep >= titles.Length)
        {
            Finish();
            return;
        }

        RenderStep();
    }

    private void RenderStep()
    {
        StepTitle.text = titles[currentStep];
        StepBody.text = bodies[currentStep];
        NextButtonLabel.text = currentStep == titles.Length - 1 ? "START" : "NEXT";

        if (StepDots != null)
        {
            for (int i = 0; i < StepDots.Count; i++)
            {
                if (StepDots[i] == null) continue;
                bool isActive = i == currentStep;
                StepDots[i].color = isActive
                    ? new Color(1f, 1f, 1f, 1f)
                    : new Color(1f, 1f, 1f, 0.3f);
            }
        }

        StepTitle.rectTransform.localScale = Vector3.one * 0.9f;
        StepTitle.rectTransform.DOScale(1f, 0.2f).SetEase(Ease.OutBack);
    }

    private void Finish()
    {
        PlayerPrefs.SetInt(PREF_TUTORIAL_SEEN, 1);
        PlayerPrefs.Save();

        Popup.Close(() =>
        {
            if (OnTutorialFinished != null) OnTutorialFinished();
        });
    }
}
