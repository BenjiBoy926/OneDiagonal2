using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnlockAnything : MonoBehaviour
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Reference to the player data to load up")]
    private PlayerData data;
    [SerializeField]
    [Tooltip("If true, the player data is saved to a file after everything is unlocked")]
    private bool save;
    #endregion

    #region Monobehaviour Messages
    // Start is called before the first frame update
    void Start()
    {
        // Set the static instance of player to the locally edited one
        PlayerData.SetInstance(data);

        // Save the player data if we want to after unlocking everything
        if (save) PlayerData.Save();

        SceneManager.LoadScene("MainMenu");
    }
    #endregion
}
