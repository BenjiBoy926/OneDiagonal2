using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    #endregion

    #region Monobehaviour Messages
    private void Start()
    {
        tutorialManager.Setup(CurrentLevelData.Tutorials);
        matrixUI.Setup();

        // As soon as the matrix is solved, update the level completion data for this level
        matrixUI.OnMatrixSolved.AddListener(() =>
        {
            CurrentLevelCompletionData.CompleteLevel(matrixUI.CurrentMoves);
            PlayerData.Save();
        });
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
