using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    #region Public Typedefs
    // So that the array on enum is property serialized
    [System.Serializable]
    public class LevelCompletionDataList
    {
        // List of completion datas
        public LevelCompletionData[] array;

        // Constructor to create the array with a defined size
        public LevelCompletionDataList(int size)
        {
            array = new LevelCompletionData[size];
        }
    }
    #endregion

    #region Private Properties
    private static PlayerData Instance
    {
        get
        {
            if (instance == null) instance = Load();
            return instance;
        }
    }
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("List of completion data for every level")]
    private ArrayOnEnum<LevelType, LevelCompletionDataList> completionDatas;
    [SerializeField]
    [Tooltip("Matrix operation types currently unlocked. Parallel to the MatrixOperation.Type enum")]
    private ArrayOnEnum<MatrixOperation.Type, bool> operationsUnlocked;
    #endregion

    #region Private Fields
    private static PlayerData instance = null;
    private static readonly string fileName = "OneDiagonalSaveData.dat";
    private static readonly string savePath = Application.persistentDataPath + fileName;
    private static readonly BinaryFormatter formatter = new BinaryFormatter();
    #endregion

    #region Constructors
    private PlayerData()
    {
        // Initialize the array on enum
        completionDatas = new ArrayOnEnum<LevelType, LevelCompletionDataList>();

        // Get a list of the level types
        LevelType[] levelTypes = (LevelType[])System.Enum.GetValues(typeof(LevelType));

        foreach(LevelType type in levelTypes)
        {
            int numLevelsOfType = LevelSettings.TotalLevelsOfType(type);
            // Set the completion data list to a new list
            completionDatas.Set(type, new LevelCompletionDataList(numLevelsOfType));

            // Set each completion data to the default
            for (int i = 0; i < numLevelsOfType; i++)
            {
                completionDatas.Get(type).array[i] = new LevelCompletionData();
            }
        }

        // Create the operations unlocked array
        operationsUnlocked = new ArrayOnEnum<MatrixOperation.Type, bool>();
    }
    #endregion

    #region Public Methods
    // Get the completion data for the specified level
    public static LevelCompletionData GetCompletionData(LevelID id) => Instance.completionDatas.Get(id.Type).array[id.Index];
    public static void UnlockOperation(MatrixOperation.Type type) => Instance.operationsUnlocked.Set(type, true);
    // Save the current instance of the player data to the file
    public static void Save()
    {
        // Note: we store the instance first because Load might open the same file,
        // resulting in a sharing violation
        PlayerData currentInstance = Instance;

        using FileStream file = new FileStream(savePath, FileMode.OpenOrCreate);
        formatter.Serialize(file, currentInstance);
    }
    public static PlayerData Load()
    {
        // Data to load from file, or gives default data
        PlayerData data;

        // If a save file exists, load it
        if (SaveFileExists())
        {
            using FileStream file = new FileStream(savePath, FileMode.Open);
            data = (PlayerData)formatter.Deserialize(file);

            // Check for discrepancies between the loaded player data and the level settings
            // If there are discrepancies, we don't have any way of resolving them, so we
            // delete the player's save file and create a new data
            
            // Get a list of the level types
            LevelType[] levelTypes = (LevelType[])System.Enum.GetValues(typeof(LevelType));

            // Loop over all level types
            foreach (LevelType type in levelTypes)
            {
                // Get the number of levels with this type
                int numLevelsOfType = LevelSettings.TotalLevelsOfType(type);

                // If the levels on the level settings are not equal to the completion datas,
                // delete the save file and create new data
                if(numLevelsOfType != data.completionDatas.Get(type).array.Length)
                {
                    Delete();
                    data = new PlayerData();
                    break;
                }
            }
        }
        // If no save file exists, create a new player data
        else data = new PlayerData();

        // Return the instance
        return data;
    }
    // Delete the current instance of the player data
    public static void Delete()
    {
        if(SaveFileExists())
        {
            File.Delete(savePath);
            instance = null;
        }
    }
    public static bool SaveFileExists() => File.Exists(savePath);
    #endregion
}
