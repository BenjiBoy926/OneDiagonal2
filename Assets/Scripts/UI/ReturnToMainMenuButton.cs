using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ReturnToMainMenuButton : OptionalConfirmButton
{
    protected override void ButtonAction()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
