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
        public LevelCompletionData[] completionData;

        // Constructor to create the array with a defined size
        public LevelCompletionDataList(int size)
        {
            completionData = new LevelCompletionData[size];
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
            int numLevelsOfType = LevelSettings.GetLevelData(type).Length;
            // Set the completion data list to a new list
            completionDatas.Set(type, new LevelCompletionDataList(numLevelsOfType));

            // Set each completion data to the default
            for (int i = 0; i < numLevelsOfType; i++)
            {
                completionDatas.Get(type).completionData[i] = new LevelCompletionData();
            }
        }

        // Create the operations unlocked array
        operationsUnlocked = new ArrayOnEnum<MatrixOperation.Type, bool>();
    }
    #endregion

    #region Public Methods
    // Get the completion data for the specified level
    public static LevelCompletionData GetCompletionData(LevelID id) => Instance.completionDatas.Get(id.Type).completionData[id.Index];
    // Save the current instance of the player data to the file
    public static void Save()
    {
        // Note: we store the instance first because Load might open the same file,
        // resulting in a sharing violation
        PlayerData currentInstance = Instance;

        FileStream file = new FileStream(savePath, FileMode.OpenOrCreate);
        formatter.Serialize(file, currentInstance);
        file.Close();
    }
    public static PlayerData Load()
    {
        // Data to load from file, or gives default data
        PlayerData data;

        // If a save file exists, load it
        if (SaveFileExists())
        {
            FileStream file = new FileStream(savePath, FileMode.Open);
            data = (PlayerData)formatter.Deserialize(file);
            file.Close();
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
