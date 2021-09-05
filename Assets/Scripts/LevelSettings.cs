using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(LevelSettings))]
public class LevelSettings : ScriptableObject
{
    #region Public Typedefs
    // This is needed for the "ArrayOnEnum" to work correctly
    [System.Serializable]
    public class LevelDataList
    {
        public List<LevelData> list;
    }
    #endregion

    #region Private Properties
    private static LevelSettings Instance
    {
        get
        {
            if(!instance)
            {
                // Load the resource
                instance = Resources.Load<LevelSettings>(nameof(LevelSettings));

                // If the instance still could not be loaded then throw an exception
                if (!instance) throw new MissingReferenceException(nameof(LevelSettings) + ": no instance of type " + nameof(LevelSettings) +
                    " could be loaded from the resources folder. Make sure an instance of type " +
                    nameof(LevelSettings) + " with the name " + nameof(LevelSettings) +
                    " exists in a folder named 'Resources'");
            }
            // If instance is not null return it
            return instance;
        }
    }
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("List of all level data for all levels")]
    private ArrayOnEnum<LevelType, LevelDataList> levelDatas;
    #endregion

    #region Private Fields
    // Singleton instance of the level settings
    private static LevelSettings instance;
    #endregion

    #region Public Methods
    public static LevelData GetLevelData(LevelID id)
    {
        return Instance.levelDatas.Get(id.Type).list[id.Index];
    }
    #endregion
}
