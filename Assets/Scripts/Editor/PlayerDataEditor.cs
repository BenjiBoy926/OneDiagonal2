using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(PlayerData))]
public class PlayerDataEditor : PropertyDrawer
{
    #region Property Drawer Overrides
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Get the data array in the array on enum
        SerializedProperty dataArray = property
            .FindPropertyRelative("completionDatas")
            .FindPropertyRelative("data");

        // Iterate over all level types
        for (int i = 0; i < dataArray.arraySize; i++)
        {
            SerializedProperty levelTypeFilter = dataArray
                .GetArrayElementAtIndex(i)
                .FindPropertyRelative(nameof(levelTypeFilter));

            // Set the string value of the type filter
            levelTypeFilter.stringValue = ((LevelType)i).ToString();
        }

        // Layout the property like normal
        EditorGUI.PropertyField(position, property, label, true);
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }
    #endregion

    #region Menu Items
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
    #endregion
}
