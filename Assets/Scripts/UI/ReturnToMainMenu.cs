using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ReturnToMainMenu : MonoBehaviour
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Parent transform for the yes-no window")]
    private Transform windowParent;
    [SerializeField]
    [Tooltip("Button to press to go back to the main menu")]
    private Button returnButton;
    #endregion

    #region Private Fields
    private GenericYesNoWindow current = null;
    #endregion

    #region Monobehaviour Messages
    private void Start()
    {
        returnButton.onClick.AddListener(CreateWindow); 
    }
    #endregion

    #region Private Methods
    private void CreateWindow()
    {
        if(!current)
        {
            current = GenericYesNoWindow.CreateFromResource(windowParent);
            current.Setup("Are you sure you want to return to the main menu?", LoadMainMenu, null, null);
        }
    }
    private void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    #endregion
}
