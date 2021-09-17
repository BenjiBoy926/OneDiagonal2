using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TutorialManager : MonoBehaviour
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Image on the root of the tutorial uis")]
    private Image rootPanel;
    [SerializeField]
    [Tooltip("Time it takes to fade the root panel out")]
    private float fadeInTime = 1f;
    [SerializeField]
    [Tooltip("Time it takes for the root panel to fade out")]
    private float fadeOutTime = 0.3f;
    #endregion

    #region Monobehaviour Callbacks
    private void Start()
    {
        TutorialData[] datas = TutorialSettings.GetTutorialsForLevel(GameplayManager.CurrentLevelID);

        // If there is some data then fade in the back panel
        if (datas != null)
        {
            rootPanel.gameObject.SetActive(true);
            rootPanel.color = Color.clear;

            // When fading is finished then setup all the uis
            rootPanel.DOColor(new Color(0f, 0f, 0f, 0.8f), fadeInTime).OnComplete(() => SetupTutorialUIs(datas));
        }
        // If there is no data make sure that the panel is inactive
        else rootPanel.gameObject.SetActive(false);
    }
    #endregion

    #region Private Methods
    private void SetupTutorialUIs(TutorialData[] data)
    {
        // Instantiate a ui for the first tutorial
        TutorialUI previousUI = TutorialUI.InstantiateFromResources(rootPanel.transform);
        TutorialUI currentUI = previousUI;
        previousUI.Open(data[0]);

        // Run a loop through all tutorials after the first one
        for (int i = 1; i < data.Length; i++)
        {
            // Create the ui, starting off disabled
            currentUI = TutorialUI.InstantiateFromResources(rootPanel.transform);
            currentUI.RootRect.gameObject.SetActive(false);

            // When the previous ui is closed, then open the next one
            previousUI.OnTutorialClosed.AddListener(() => currentUI.Open(data[i]));

            // Before the loop restarts, set previous ui to the current ui
            previousUI = currentUI;
        }

        // When the last tutorial is closed then finish the tutorial
        currentUI.OnTutorialClosed.AddListener(Finish);
    }
    private void Finish()
    {
        rootPanel.DOColor(Color.clear, fadeOutTime).OnComplete(() => rootPanel.gameObject.SetActive(false));
    }
    #endregion
}
