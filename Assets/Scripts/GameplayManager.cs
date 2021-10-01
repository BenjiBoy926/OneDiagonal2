using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameplayManager : MonoBehaviour
{
    #region Public Properties
    // The current level being played
    public static LevelID CurrentLevelID { get; private set; } = new LevelID(LevelType.Enumerated, 0);
    public static LevelData CurrentLevelData => LevelSettings.GetLevelData(CurrentLevelID);
    public static LevelCompletionData CurrentLevelCompletionData => PlayerData.GetCompletionData(CurrentLevelID);
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Reference to the manager used to setup tutorials")]
    private TutorialManager tutorialManager;
    [SerializeField]
    [Tooltip("Reference to the matrix ui to setup at the start")]
    private MatrixUI matrixUI;
    [SerializeField]
    [Tooltip("Text that displays the current level being played")]
    private TextMeshProUGUI levelTitle;
    #endregion

    #region Monobehaviour Messages
    private void Start()
    {
        // Only display the tutorial if it has not been encountered yet
        if(!CurrentLevelCompletionData.Encountered)
        {
            tutorialManager.OpenTutorials(CurrentLevelData.Tutorials, true);
        }
        // As soon as the level starts, this level is marked as "encountered"
        CurrentLevelCompletionData.EncounterLevel();
        PlayerData.Save();

        matrixUI.Setup();

        // As soon as the matrix is solved, update the level completion data for this level
        matrixUI.OnMatrixSolved.AddListener(() =>
        {
            CurrentLevelCompletionData.CompleteLevel(matrixUI.CurrentMoves);
            PlayerData.Save();
        });

        // Set the text to show the name of the level
        levelTitle.text = CurrentLevelID.Data.Name;
    }
    #endregion

    #region Public Methods
    public static void PlayLevel(LevelID level)
    {
        CurrentLevelID = level;
        SceneManager.LoadScene("GameplayScene");
    }
    public static void ReplayLevel() => PlayLevel(CurrentLevelID);
    public static void PlayNextLevel() => PlayLevel(new LevelID(CurrentLevelID.Type, CurrentLevelID.Index + 1));
    #endregion
}
