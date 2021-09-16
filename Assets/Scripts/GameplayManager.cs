using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : MatrixUIChild
{
    #region Public Properties
    // The current level being played
    public static LevelID CurrentLevelID { get; private set; } = new LevelID(LevelType.Enumerated, 0);
    public static LevelData CurrentLevelData => LevelSettings.GetLevelData(CurrentLevelID);
    public static LevelCompletionData CurrentLevelCompletionData => PlayerData.GetCompletionData(CurrentLevelID);
    #endregion

    #region Monobehaviour Messages
    protected override void Start()
    {
        base.Start();

        // As soon as the matrix is solved, update the level completion data for this level
        MatrixParent.OnMatrixSolved.AddListener(() =>
        {
            CurrentLevelCompletionData.CompleteLevel(MatrixParent.CurrentMoves);
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
