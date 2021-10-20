using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

public class TutorialManager : MonoBehaviour
{
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

    #region Public Methods
    public void OpenTutorials(TutorialData[] tutorials, bool displayUpgrades)
    {
        // If there is some data then fade in the back panel
        if (tutorials.Length > 0)
        {
            rootPanel.gameObject.SetActive(true);
            rootPanel.color = Color.clear;

            // At the very start, unlock all upgrades in all tutorials
            foreach (TutorialData tutorial in tutorials)
            {
                tutorial.OptionalUnlockData.TryUnlock();
            }

            // When fading is finished then setup all the uis
            rootPanel.DOColor(new Color(0f, 0f, 0f, 0.8f), fadeInTime).OnComplete(() => SetupTutorialUIs(tutorials, displayUpgrades));
        }
        // If there is no data make sure that the panel is inactive
        else rootPanel.gameObject.SetActive(false);
    }
    #endregion

    #region Private Methods
    private void SetupTutorialUIs(TutorialData[] tutorials, bool displayUpgrades)
    {
        // Local function returns the functor that opens the correct tutorial
        // this prevents capturing local variables
        UnityAction OpenTutorial(TutorialUI tutorial, TutorialData data)
        {
            return () => tutorial.Open(data, displayUpgrades);
        }

        // The current and previous UI
        TutorialUI current = TutorialUI.InstantiateFromResources(rootPanel.transform);
        TutorialUI previous = current;

        // Open the current UI
        current.Open(tutorials[0], displayUpgrades);

        // Run a loop through all tutorials after the first one
        for (int i = 1; i < tutorials.Length; i++)
        {
            // Create the ui, starting off disabled
            current = TutorialUI.InstantiateFromResources(rootPanel.transform);
            current.RootRect.gameObject.SetActive(false);

            // When the previous ui is closed, then open the next one
            previous.OnTutorialClosed.AddListener(OpenTutorial(current, tutorials[i]));

            // Update previous to current before moving on
            previous = current;
        }

        // When the last tutorial is closed then finish the tutorial
        current.OnTutorialClosed.AddListener(Finish);
    }
    private void Finish()
    {
        rootPanel.DOColor(Color.clear, fadeOutTime).OnComplete(() => rootPanel.gameObject.SetActive(false));
    }
    #endregion
}
