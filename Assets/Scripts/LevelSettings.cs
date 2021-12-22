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

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("List of all level data for all levels")]
    private ArrayOnEnum<LevelType, LevelDataList> levelDatas;
    #endregion

    #region Public Methods
    public static int TotalLevelsOfType(LevelType type) => Instance.levelDatas.Get(type).list.Length;
    public static LevelData GetLevelData(LevelID id)
    {
        LevelData[] list = Instance.levelDatas.Get(id.Type).list;
        if (id.Index >= 0 && id.Index < list.Length) return list[id.Index];
        else return null;
    }
    public static bool IsLastLevel(LevelID id) => id.Index == (Instance.levelDatas.Get(id.Type).list.Length - 1);
    public static LevelID[] GetAllLevelIDsOfType(LevelType type)
    {
        LevelID[] ids = new LevelID[Instance.levelDatas.Get(type).list.Length];

        for(int i = 0; i < ids.Length; i++)
        {
            ids[i] = new LevelID(type, i);
        }

        return ids;
    }
    #endregion
}
