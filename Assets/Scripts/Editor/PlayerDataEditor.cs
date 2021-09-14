using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerDataEditor
{
    [MenuItem("File/Delete Save Data")]
    static void DeletePlayerData()
    {
        PlayerData.Delete();
    }
    [MenuItem("File/Delete Save Data", true)]
    static bool ValidateDeletePlayerData()
    {
        return PlayerData.SaveFileExists();
    }
}
