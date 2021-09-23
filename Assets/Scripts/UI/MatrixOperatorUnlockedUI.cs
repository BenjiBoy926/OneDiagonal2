using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrixOperatorUnlockedUI : MonoBehaviour
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Canvas group to contorl whether all of the controls in the children will be interactable or not")]
    private CanvasGroup group;
    [SerializeField]
    [Tooltip("Operation to check and see if it is unlocked")]
    private MatrixOperation.Type operation;
    [SerializeField]
    [Tooltip("Alpha of the group when these operations are not unlocked")]
    private float disabledAlpha = 0.3f;
    #endregion

    #region Monobehaviour Messages
    private void Start()
    {
        if(PlayerData.OperationUnlocked(operation))
        {
            group.alpha = 1f;
            group.blocksRaycasts = true;
        }
        else
        {
            group.alpha = disabledAlpha;
            group.blocksRaycasts = false;
        }
    }
    #endregion
}
