using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    #region Private Properties
    private static PlayerData Instance
    {
        get
        {
            if (instance == null) SetInstanceFromFile();
            return instance;
        }
    }
    // This has to be a property instead of a readonly field
    // because of serialization problems
    private static string SavePath => Application.persistentDataPath + fileName;
    #endregion

    #region Public Properties
    public static bool FreePlayUnlocked
    {
        get => Instance.freePlayUnlocked;
        set => Instance.freePlayUnlocked = value;
    }
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("List of completion data for every level")]
    private ArrayOnEnum<LevelType, LevelCompletionDataList> completionDatas;
    [SerializeField]
    [Tooltip("Matrix operation types currently unlocked. Parallel to the MatrixOperation.Type enum")]
    private ArrayOnEnum<MatrixOperation.Type, bool> operationsUnlocked;
    [SerializeField]
    [Tooltip("True if free play has been unlocked")]
    private bool freePlayUnlocked = false;
    #endregion

    #region Private Fields
    private static PlayerData instance = null;
    private static readonly string fileName = "OneDiagonalSaveData.dat";
    private static readonly BinaryFormatter formatter = new BinaryFormatter();
    #endregion

    #region Public Methods
    public static PlayerData Create()
    {
        // Create a player data object
        PlayerData data = new PlayerData();

        // Initialize the array on enum
        data.completionDatas = new ArrayOnEnum<LevelType, LevelCompletionDataList>();

        // Get a list of the level types
        LevelType[] levelTypes = (LevelType[])System.Enum.GetValues(typeof(LevelType));

        foreach (LevelType type in levelTypes)
        {
            int numLevelsOfType = LevelSettings.TotalLevelsOfType(type);
            // Set the completion data list to a new list
            data.completionDatas.Set(type, new LevelCompletionDataList(numLevelsOfType));

            // Set each completion data to the default
            for (int i = 0; i < numLevelsOfType; i++)
            {
                data.completionDatas.Get(type).Array[i] = new LevelCompletionData();
            }
        }

        // Create the operations unlocked array
        data.operationsUnlocked = new ArrayOnEnum<MatrixOperation.Type, bool>();

        // Return the data that was set up
        return data;
    }
    public static void SetInstanceFromFile() => SetInstance(Load());
    public static void SetInstance(PlayerData newInstance) => instance = newInstance;
    // Get the completion data for the specified level
    public static LevelCompletionData GetCompletionData(LevelID id) => Instance.completionDatas.Get(id.Type).Array[id.Index];
    public static LevelCompletionData[] GetCompletionDatasWithType(LevelType levelType) => Instance.completionDatas.Get(levelType).Array;
    public static bool OperationUnlocked(MatrixOperation.Type type) => Instance.operationsUnlocked.Get(type);
    public static void UnlockOperation(MatrixOperation.Type type) => Instance.operationsUnlocked.Set(type, true);
    public static void UnlockAllOperations()
    {
        MatrixOperation.Type[] operations = (MatrixOperation.Type[])System.Enum.GetValues(typeof(MatrixOperation.Type));

        foreach(MatrixOperation.Type type in operations)
        {
            UnlockOperation(type);
        }
    }
    // Save the current instance of the player data to the file
    public static void Save()
    {
        // Note: we store the instance first because Load might open the same file,
        // resulting in a sharing violation
        PlayerData currentInstance = Instance;

        using FileStream file = new FileStream(SavePath, FileMode.OpenOrCreate);
        formatter.Serialize(file, currentInstance);
    }
    public static PlayerData Load()
    {
        // Data to load from file, or gives default data
        PlayerData data;

        // If a save file exists, load it
        if (SaveFileExists())
        {
            // Use the file to deserialize the data
            using (FileStream file = new FileStream(SavePath, FileMode.Open))
            {
                data = (PlayerData)formatter.Deserialize(file);
            }

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
                if(numLevelsOfType != data.completionDatas.Get(type).Array.Length)
                {
                    Delete();
                    data = Create();
                    break;
                }
            }
        }
        // If no save file exists, create a new player data
        else data = Create();

        // Return the instance
        return data;
    }
    // Delete the current instance of the player data
    public static void Delete()
    {
        if(SaveFileExists())
        {
            File.Delete(SavePath);
            instance = null;
        }
    }
    public static bool SaveFileExists() => File.Exists(SavePath);
    #endregion
}
