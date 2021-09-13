using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(LevelSettings))]
public class LevelSettings : ScriptableObjectSingleton<LevelSettings>
{
    #region Public Typedefs
    // This is needed for the "ArrayOnEnum" to work correctly
    [System.Serializable]
    public class LevelDataList
    {
        public List<LevelData> list;
    }
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("List of all level data for all levels")]
    private ArrayOnEnum<LevelType, LevelDataList> levelDatas;
    #endregion

    #region Public Methods
    public static LevelData GetLevelData(LevelID id)
    {
        return Instance.levelDatas.Get(id.Type).list[id.Index];
    }
    #endregion
}
