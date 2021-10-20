using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Button that allows the player to select an enumerated level, or starts the first level if there are none")]
    private Button playButton;
    [SerializeField]
    [Tooltip("Button that takes the player to the level select scene")]
    private Button levelSelectButton;
    [SerializeField]
    [Tooltip("Button that allows the player to select a free play level")]
    private Button freePlayButton;
    [SerializeField]
    [Tooltip("Button that deletes the data when confirmed")]
    private DeleteDataButton deleteDataButton;

    [Space]

    [SerializeField]
    [Tooltip("Manages the display of the tutorial")]
    private TutorialManager tutorialManager;
    [SerializeField]
    [Tooltip("Tutorial that runs when the player unlocks free play mode for the first time")]
    private TutorialData freePlayTutorial;
    #endregion

    #region Monobehaviour Messages
    private void Start()
    {
        // TEMP
        //PlayerData.UnlockOperation(MatrixOperation.Type.Add);

        bool anyLevelsCompleted = PlayerData
            .GetCompletionDatasWithType(LevelType.Enumerated)
            .Any(x => x.Completed);
        bool allLevelsCompleted = PlayerData
            .GetCompletionDatasWithType(LevelType.Enumerated)
            .All(x => x.Completed);
        LevelID firstIncompleteLevel = GetFirstIncompleteLevel();

        // If all levels are completed then unlock free play mode
        if(allLevelsCompleted && !PlayerData.FreePlayUnlocked)
        {
            // Unlock free play mode
            PlayerData.FreePlayUnlocked = true;
            PlayerData.Save();

            // Display a tutorial for free play
            tutorialManager.OpenTutorials(new TutorialData[] { freePlayTutorial }, true);
        }

        // Play button plays the first incomplete level, or goes to level selector if all are completed
        playButton.onClick.AddListener(() =>
        {
            if (firstIncompleteLevel.IsValid) GameplayManager.PlayLevel(firstIncompleteLevel);
            else LevelSelector.SelectLevelsWithType(LevelType.Enumerated);
        });

        // Level select button selects levels only if some have been completed
        levelSelectButton.interactable = anyLevelsCompleted;
        if (anyLevelsCompleted) levelSelectButton.onClick.AddListener(() => LevelSelector.SelectLevelsWithType(LevelType.Enumerated));

        // Free play only unlocked if all levels are completed
        freePlayButton.interactable = allLevelsCompleted;
        if (allLevelsCompleted) freePlayButton.onClick.AddListener(() => LevelSelector.SelectLevelsWithType(LevelType.FreePlay));

        // When delete data is confirmed, reload this scene
        deleteDataButton.OnConfirm.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name));
    }
    #endregion

    #region Private Method
    private LevelID GetFirstIncompleteLevel()
    {
        // Get a list of all incompleted levels
        IEnumerable<(LevelCompletionData data, int index)> incompleteLevels = PlayerData
            .GetCompletionDatasWithType(LevelType.Enumerated)
            .Select((data, index) => (data, index))
            .Where(x => !x.data.Completed);

        // If there are incompleted levels, get the ID of the first one
        if (incompleteLevels.Count() > 0)
        {
            int index = incompleteLevels.First().index;
            return new LevelID(LevelType.Enumerated, index);
        }
        // If all levels are completed, return the invalid ID
        else return LevelID.Invalid;
    }
    #endregion
}
