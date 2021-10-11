using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameplayManager))]
public class GameplayManagerEditor : Editor
{
    #region Public Methods
    public override void OnInspectorGUI()
    {
        // Get all sub properties
        SerializedProperty tutorialManager = serializedObject.FindProperty(nameof(tutorialManager));
        SerializedProperty matrixUI = serializedObject.FindProperty(nameof(matrixUI));
        SerializedProperty levelTitle = serializedObject.FindProperty(nameof(levelTitle));
        SerializedProperty setLevelIDOnAwake = serializedObject.FindProperty(nameof(setLevelIDOnAwake));
        SerializedProperty levelID = serializedObject.FindProperty(nameof(levelID));

        // Update visual
        serializedObject.Update();

        // Edit the basic properties
        EditorGUILayout.PropertyField(tutorialManager);
        EditorGUILayout.PropertyField(matrixUI);
        EditorGUILayout.PropertyField(levelTitle);

        // Edit play on awake
        EditorGUILayout.PropertyField(setLevelIDOnAwake);

        // Only edit the level id is we will play on awake
        if(setLevelIDOnAwake.boolValue)
        {
            EditorGUILayout.PropertyField(levelID);
        }

        serializedObject.ApplyModifiedProperties();
    }
    #endregion
}
