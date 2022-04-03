using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EffectConfig
{
    #region Public Properties
    public FlashEffect FlashEffectPrefab => flashEffectPrefab;
    public OutlineEffect OutlineEffectPrefab => outlineEffectPrefab;
    public int InitialSize => initialSize;
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Reference to the flash to use")]
    private FlashEffect flashEffectPrefab;
    [SerializeField]
    [Tooltip("Reference to the outline to use")]
    private OutlineEffect outlineEffectPrefab;
    [SerializeField]
    [Tooltip("Initial size of the pool")]
    private int initialSize = 2;
    #endregion
}
