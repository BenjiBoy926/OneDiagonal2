using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnlockOperation
{
    #region Public Properties
    public MatrixOperation.Type OperationType => operationType;
    public Sprite UnlockSprite => unlockSprite;
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("The type of operation to unlock")]
    private MatrixOperation.Type operationType;
    [SerializeField]
    [Tooltip("The sprite to display when unlocking this operation")]
    private Sprite unlockSprite;
    #endregion

    #region Public Methods
    public void Unlock()
    {
        PlayerData.UnlockOperation(operationType);
    }
    #endregion
}
