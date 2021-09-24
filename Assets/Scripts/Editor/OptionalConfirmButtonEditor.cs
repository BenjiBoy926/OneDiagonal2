using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class OptionalConfirmButtonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Get require confirmation property
        SerializedProperty requireConfirmation = serializedObject.FindProperty(nameof(requireConfirmation));

        serializedObject.Update();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("actionButton"));
        EditorGUILayout.PropertyField(requireConfirmation);

        if(requireConfirmation.boolValue)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("windowParent"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("confirmationMessage"));
            EditorGUI.indentLevel--;
        }

        serializedObject.ApplyModifiedProperties();
    }
}

[CustomEditor(typeof(ReturnToMainMenuButton))]
public class ReturnToMainMenuButtonEditor : OptionalConfirmButtonEditor { }

[CustomEditor(typeof(ReplayLevelButton))]
public class ReplayLevelButtonEditor : OptionalConfirmButtonEditor { }
