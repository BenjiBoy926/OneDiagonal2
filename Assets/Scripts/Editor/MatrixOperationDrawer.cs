using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(MatrixOperation))]
public class MatrixOperationDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty type = property.FindPropertyRelative(nameof(type));
        SerializedProperty destinationRow = property.FindPropertyRelative(nameof(destinationRow));
        SerializedProperty sourceRow = property.FindPropertyRelative(nameof(sourceRow));
        SerializedProperty scalar = property.FindPropertyRelative(nameof(scalar));

        // Set height for just one control
        position.height = LayoutUtilities.standardControlHeight;

        // Edit the type
        EditorGUI.PropertyField(position, type, label);
        position.y += position.height;

        // Add indent
        EditorGUI.indentLevel++;

        // Edit the destination row
        EditorGUI.PropertyField(position, destinationRow);
        position.y += position.height;

        // Change properties edited based on enum value index
        switch (type.enumValueIndex)
        {
            case 0:
                EditorGUI.PropertyField(position, sourceRow);
                break;
            case 1:
                EditorGUI.PropertyField(position, scalar);
                break;
            case 2:
                EditorGUI.PropertyField(position, sourceRow);
                position.y += position.height;
                EditorGUI.PropertyField(position, scalar);
                break;
        }

        // Remove indent
        EditorGUI.indentLevel--;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty type = property.FindPropertyRelative(nameof(type));

        if (type.enumValueIndex == 2) return LayoutUtilities.standardControlHeight * 4f;
        else return LayoutUtilities.standardControlHeight * 3f;
    }
}
