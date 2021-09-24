using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialButton : MonoBehaviour
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Reference to the tutorial manager that displays tutorials")]
    private TutorialManager tutorialManager;
    [SerializeField]
    [Tooltip("Reference to the button that displays the tutorials again when clicked")]
    private Button button;
    #endregion

    #region Monobehavour Messages
    private void Start()
    {
        if (GameplayManager.CurrentLevelData.Tutorials.Length > 0)
        {
            // When button is clicked then setup the tutorial manager without upgrade display
            button.interactable = true;
            button.onClick.AddListener(() =>
            {
                tutorialManager.OpenTutorials(GameplayManager.CurrentLevelData.Tutorials, false);
            });
        }
        // If the level has no tutorials then this button is not interactable
        else button.interactable = false;
    }
    #endregion
}
