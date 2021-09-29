using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LevelID
{
    #region Public Properties
    public LevelType Type => type;
    public int Index => index;
    public LevelData Data => LevelSettings.GetLevelData(this);
    public bool IsValid => Data != null;
    public static LevelID Invalid => new LevelID(0, -1);
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

    #region Overrides
    public override bool Equals(object obj)
    {
        if (obj == null) return false;
        else if (obj.GetType() == GetType()) return this == (LevelID)obj;
        else return false;
    }
    public override int GetHashCode()
    {
        return type.GetHashCode() + index.GetHashCode();
    }
    public override string ToString()
    {
        return $"Level ID: {type}, {index}";
    }
    #endregion

    #region Operators
    public static bool operator==(LevelID a, LevelID b) => a.type == b.type && a.index == b.index;
    public static bool operator !=(LevelID a, LevelID b) => !(a == b);
    #endregion
}
