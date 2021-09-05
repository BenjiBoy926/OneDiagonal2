using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    #region Public Properties
    // The current level being played
    public static LevelID CurrentLevel { get; private set; } = new LevelID(LevelType.Enumerated, 0);
    #endregion

    #region Public Methods
    public static void PlayLevel(LevelID level)
    {
        CurrentLevel = level;
        SceneManager.LoadScene("GameplayScene");
    }
    #endregion
}
