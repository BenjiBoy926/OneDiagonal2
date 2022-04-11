using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialDropdown : MonoBehaviour
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Reference to the script that can open up tutorials")]
    private TutorialManager tutorialManager;
    [SerializeField]
    [Tooltip("Reference to the object that creates the dropdown")]
    private GenericDropdown dropdown;
    [SerializeField]
    [Tooltip("Parent of the buttons that open the correct tutorial")]
    private Transform buttonParent;
    [SerializeField]
    [Tooltip("Button prefab to use to create buttons")]
    private TutorialButton buttonPrefab;
    #endregion

    #region Monobehavour Messages
    private void Start()
    {
        // Get all the levels
        LevelID[] ids = LevelSettings.GetAllLevelIDs();
        foreach (LevelID id in ids)
        {
            LevelData data = LevelSettings.GetLevelData(id);
            if (data.Tutorials.Length > 0)
            {
                foreach (TutorialData tutorial in data.Tutorials)
                {
                    TutorialButton button = Instantiate(buttonPrefab, buttonParent);
                    button.TutorialManager = tutorialManager;
                    button.Tutorial = tutorial;
                    button.Button.interactable = PlayerData.GetCompletionData(id).Encountered;

                    if (button.Button.interactable)
                        dropdown.AddDropdownDisableButton(button.Button);
                }
            }
        }
    }
    #endregion
}
