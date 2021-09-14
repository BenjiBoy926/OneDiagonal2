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
    #endregion

    #region Public Methods
    public static void PlayLevel(LevelID level)
    {
        CurrentLevelID = level;
        SceneManager.LoadScene("GameplayScene");
    }
    #endregion
}
