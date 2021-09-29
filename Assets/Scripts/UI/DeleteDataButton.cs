using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteDataButton : OptionalConfirmButton
{
    #region Monobehaviour Messages
    protected override void Start()
    {
        base.Start();
        actionButton.interactable = PlayerData.SaveFileExists();
    }
    #endregion

    #region Protected Methods
    protected override void ButtonAction()
    {
        PlayerData.Delete();
        base.ButtonAction();
    }
    #endregion
}
