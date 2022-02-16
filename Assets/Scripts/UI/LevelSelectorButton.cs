using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSelectorButton : MonoBehaviour
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Reference to the button that loads the given level")]
    private Button button;
    [SerializeField]
    [Tooltip("Text to display on this button")]
    private TextMeshProUGUI text;
    [SerializeField]
    [Tooltip("If true, this level can only be selected if the level before it has been completed")]
    private bool basedOnCompletion = true;
    #endregion

    #region Public Methods
    public void Setup(LevelID levelID)
    {
        // Check if the interactability is based on completion or not
        if (basedOnCompletion)
        {
            // Get the id of the previous level
            LevelID previous = new LevelID(levelID.Type, levelID.Index - 1);

            // If it is valid, the button is interactable if the previous level has been completed
            if (previous.IsValid)
            {
                button.interactable = PlayerData.GetCompletionData(previous).Completed;
            }
            else button.interactable = true;
        }
        else button.interactable = true;

        // Add listener to the button if it is interactable
        if (button.interactable) button.onClick.AddListener(() => GameplayManager.PlayLevel(levelID));

        // Set the text on the component
        text.text = levelID.Data.Name;
    }
    #endregion
}
