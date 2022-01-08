using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnlockEverything : MonoBehaviour
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("If true, the player data is saved to a file after everything is unlocked")]
    private bool save;
    #endregion

    #region Monobehaviour Messages
    // Start is called before the first frame update
    void Start()
    {
        // Go through all level types, completing all levels of that type
        LevelType[] types = (LevelType[])System.Enum.GetValues(typeof(LevelType));

        foreach(LevelType type in types)
        {
            // Get the list of all levels with this data
            LevelCompletionData[] datas = PlayerData.GetCompletionDatasWithType(type);

            // Complete every level
            foreach(LevelCompletionData data in datas)
            {
                data.CompleteLevel(1000);
            }
        }

        // Unlock every matrix operation
        PlayerData.UnlockAllOperations();

        // Unlock free play mode
        PlayerData.FreePlayUnlocked = true;

        // Save the player data if we want to after unlocking everything
        if (save) PlayerData.Save();

        SceneManager.LoadScene("MainMenu");
    }
    #endregion
}
