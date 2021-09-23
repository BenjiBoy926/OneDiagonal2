using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnlockOperation
{
    #region Public Properties
    public MatrixOperation.Type OperationType => operationType;
    public Sprite UnlockSprite => unlockSprite;
    public string UnlockItem => unlockItem;
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("The type of operation to unlock")]
    private MatrixOperation.Type operationType;
    [SerializeField]
    [Tooltip("The sprite to display when unlocking this operation")]
    private Sprite unlockSprite;
    [SerializeField]
    [Tooltip("The name identifier of the item being unlocked")]
    private string unlockItem;
    #endregion

    #region Public Methods
    public void Unlock()
    {
        PlayerData.UnlockOperation(operationType);
    }
    #endregion
}
