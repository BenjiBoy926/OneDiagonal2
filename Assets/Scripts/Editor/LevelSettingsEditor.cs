using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelSettings))]
public class LevelSettingsEditor : Editor
{
    #region Private Fields
    private bool levelNameChangeFoldout;
    private LevelType levelType;
    #endregion

    #region Private Constants
    private readonly GUIContent levelNameChangeContent = new GUIContent(
            "Change level names",
            "Change all the names of all the levels of the given type " +
            "to have the form Level 1, Level 2, Level 3, " +
            "and so on for each level in the array");
    private readonly GUIContent levelTypeContent = new GUIContent(
        "Level Type",
        "All levels of this type with have their names changed to have the form " +
        "Level 1, Level 2, Level 3, and so on for each level of this type");
    private const string levelNameChangeOptOutKey = "levelNameChange";
    #endregion

    #region Editor Overrides
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawDefaultInspector();

        // Setup the content for the level name change 
        levelNameChangeFoldout = EditorGUILayout.Foldout(levelNameChangeFoldout, levelNameChangeContent);

        // If the name change is folded out then display an enum popup for the type to change
        if (levelNameChangeFoldout)
        {
            // Increase indent for foldout elements
            EditorGUI.indentLevel++;

            // Layout the prefix and enum popup
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(levelTypeContent);
            levelType = (LevelType)EditorGUILayout.EnumPopup(levelType);
            EditorGUILayout.EndHorizontal();

            // Add a button to change the level names
            if (GUILayout.Button("Change level names"))
            {
                // Display a dialog to confirm that they want to overwrite.
                // They can also opt-out if desired
                if (EditorUtility.DisplayDialog("Change level names",
                    $"Overwrite existing level names?  " +
                    $"Level names will be in the form Level 1, Level 2, Level 3, " +
                    $"and so on for each level of type '{levelType}'",
                    "Overwrite level names", "Keep old level names",
                    DialogOptOutDecisionType.ForThisSession, 
                    levelNameChangeOptOutKey))
                {
                    SetLevelNames(levelType);
                }
            }

            EditorGUI.indentLevel--;
        }

        serializedObject.ApplyModifiedProperties();
    }
    #endregion

    #region Private Methods
    private void SetLevelNames(LevelType levelType)
    {
        // Get the list of level data associated with this type
        int enumIndex = (int)levelType;
        SerializedProperty list = serializedObject.FindProperty("levelDatas")
            .FindPropertyRelative("data")
            .GetArrayElementAtIndex(enumIndex)
            .FindPropertyRelative("list");

        // Set the level names of all level datas in the list for this level type
        for(int i = 0; i < list.arraySize; i++)
        {
            SerializedProperty levelName = list.GetArrayElementAtIndex(i)
                .FindPropertyRelative("name");
            levelName.stringValue = $"Level {i + 1}";
        }
    }
    #endregion
}
