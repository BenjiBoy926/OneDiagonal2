using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OptionalUnlockOperation
{
    #region Public Properties
    public bool WillUnlock => willUnlock;
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("True if the unlock will take place and false if not")]
    private bool willUnlock;
    [SerializeField]
    [Tooltip("The operation to unlock")]
    private UnlockOperation unlocker;
    #endregion

    #region Public Methods
    public bool TryUnlock()
    {
        if (willUnlock) unlocker.Unlock();
        return willUnlock;
    }
    public UnlockOperation TryGetUnlocker()
    {
        if (willUnlock) return unlocker;
        else return null;
    }
    #endregion
}
