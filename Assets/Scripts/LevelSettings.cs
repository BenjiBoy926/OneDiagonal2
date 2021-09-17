using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(LevelSettings))]
public class LevelSettings : ScriptableObjectSingleton<LevelSettings>
{
    #region Public Typedefs
    // This is needed for the "ArrayOnEnum" to be serialized correctly
    [System.Serializable]
    public class LevelDataList
    {
        public LevelData[] list;
    }
    #endregion

    #region Public Properties
    public static int TotalLevels
    {
        get
        {
            int total = 0;
            foreach(LevelDataList list in Instance.levelDatas.Data)
            {
                total += list.list.Length;
            }
            return total;
        }
    }
    #endregion

    #region Private Properties
    private static LevelSettings Instance => GetOrCreateInstance(nameof(LevelSettings), nameof(LevelSettings));
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("List of all level data for all levels")]
    private ArrayOnEnum<LevelType, LevelDataList> levelDatas;
    #endregion

    #region Public Methods
    public static int TotalLevelsOfType(LevelType type) => Instance.levelDatas.Get(type).list.Length;
    public static LevelData GetLevelData(LevelID id) => Instance.levelDatas.Get(id.Type).list[id.Index];
    public static bool IsLastLevel(LevelID id) => id.Index == (Instance.levelDatas.Get(id.Type).list.Length - 1);
    #endregion
}
