using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

public class TutorialManager : MonoBehaviour
{
    #region Public Properties
    public static TutorialData[] CurrentTutorials => GameplayManager.CurrentLevelData.Tutorials; 
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Image on the root of the tutorial uis")]
    private Image rootPanel;
    [SerializeField]
    [Tooltip("Time it takes to fade the root panel in")]
    private float fadeInTime = 1f;
    [SerializeField]
    [Tooltip("Time it takes for the root panel to fade out")]
    private float fadeOutTime = 0.3f;
    #endregion

    #region Monobehaviour Callbacks
    private void Start()
    {
        // If there is some data then fade in the back panel
        if (CurrentTutorials.Length > 0)
        {
            rootPanel.gameObject.SetActive(true);
            rootPanel.color = Color.clear;

            // When fading is finished then setup all the uis
            rootPanel.DOColor(new Color(0f, 0f, 0f, 0.8f), fadeInTime).OnComplete(() => SetupTutorialUIs());
        }
        // If there is no data make sure that the panel is inactive
        else rootPanel.gameObject.SetActive(false);
    }
    #endregion

    #region Private Methods
    private void SetupTutorialUIs()
    {
        // An array of uis for each tutorial
        TutorialUI[] uis = new TutorialUI[CurrentTutorials.Length];
        uis[0] = TutorialUI.InstantiateFromResources(rootPanel.transform);
        uis[0].Open(CurrentTutorials[0]);

        // Local function returns the functor that opens the correct tutorial
        // this prevents capturing of the "i" variable
        UnityAction OpenTutorial(int i)
        {
            return () => uis[i].Open(CurrentTutorials[i]);
        }

        // Run a loop through all tutorials after the first one
        for (int i = 1; i < CurrentTutorials.Length; i++)
        {
            // Create the ui, starting off disabled
            uis[i] = TutorialUI.InstantiateFromResources(rootPanel.transform);
            uis[i].RootRect.gameObject.SetActive(false);

            // When the previous ui is closed, then open the next one
            uis[i - 1].OnTutorialClosed.AddListener(OpenTutorial(i));
        }

        // When the last tutorial is closed then finish the tutorial
        uis[uis.Length - 1].OnTutorialClosed.AddListener(Finish);
    }
    private void Finish()
    {
        rootPanel.DOColor(Color.clear, fadeOutTime).OnComplete(() => rootPanel.gameObject.SetActive(false));
    }
    #endregion
}
