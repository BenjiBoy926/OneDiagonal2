using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReplayLevelButton : OptionalConfirmButton
{
    protected override void ButtonAction()
    {
        GameplayManager.ReplayLevel();
        base.ButtonAction();
    }
}
