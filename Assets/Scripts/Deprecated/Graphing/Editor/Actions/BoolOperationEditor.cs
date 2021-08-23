using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(BoolOperation))]
[CanEditMultipleObjects]
public class BoolOperationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SerializedProperty type = serializedObject.FindProperty("type");

        serializedObject.Update();

        EditorGUILayout.PropertyField(type, true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("input1"), true);

        if(type.enumValueIndex != 2)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("input2"), true);
        }

        EditorGUILayout.PropertyField(serializedObject.FindProperty("result"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("output"), true);

        serializedObject.ApplyModifiedProperties();
    }
}
