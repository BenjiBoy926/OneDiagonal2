using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LevelID
{
    #region Public Properties
    public LevelType Type => type;
    public int Index => index;
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Type of level to get")]
    private LevelType type;
    [SerializeField]
    [Tooltip("Index in the settings to find the level")]
    private int index;
    #endregion

    #region Constructors
    public LevelID(LevelType type, int index)
    {
        this.type = type;
        this.index = index;
    }
    #endregion
}
