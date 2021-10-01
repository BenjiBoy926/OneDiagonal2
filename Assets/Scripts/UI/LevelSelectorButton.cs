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

        // Setup the text of the button
        if (levelID.Type == LevelType.Enumerated)
        {
            text.text = (levelID.Index + 1).ToString();
        }
        else text.text = levelID.Data.Name;

        if(levelID.Type == LevelType.FreePlay)
        {
            Color darkGreen = new Color(0.1f, 0.3f, 0.1f);
            Color darkRed = new Color(0.3f, 0.1f, 0.1f);
            float t = (float)levelID.Index / (LevelSettings.GetAllLevelIDsOfType(LevelType.FreePlay).Length - 1);
            button.targetGraphic.color = Color.Lerp(darkGreen, darkRed, t);
        }
    }
    #endregion
}
