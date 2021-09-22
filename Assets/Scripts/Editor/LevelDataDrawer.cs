using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(LevelData))]
public class LevelDataDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty size = property.FindPropertyRelative(nameof(size));
        SerializedProperty type = property.FindPropertyRelative(nameof(type));
        SerializedProperty intendedSolution = property.FindPropertyRelative(nameof(intendedSolution));
        SerializedProperty tutorials = property.FindPropertyRelative(nameof(tutorials));

        // Set height for only one control
        position.height = LayoutUtilities.standardControlHeight;

        // Layout the foldout
        property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label);
        position.y += position.height;

        if(property.isExpanded)
        {
            EditorGUI.indentLevel++;

            // Edit the size
            EditorGUI.PropertyField(position, size);
            position.y += position.height;
            // Edit the type
            EditorGUI.PropertyField(position, type);
            position.y += position.height;

            // If type is fixed then edit the list
            if(type.enumValueIndex == 0)
            {
                EditorGUI.PropertyField(position, intendedSolution, true);
                position.y += EditorGUI.GetPropertyHeight(intendedSolution, true);
            }

            // Edit the tutorials
            EditorGUI.PropertyField(position, tutorials, true);

            EditorGUI.indentLevel--;
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty type = property.FindPropertyRelative(nameof(type));
        SerializedProperty intendedSolution = property.FindPropertyRelative(nameof(intendedSolution));
        SerializedProperty tutorials = property.FindPropertyRelative(nameof(tutorials));

        float height = LayoutUtilities.standardControlHeight;

        if(property.isExpanded)
        {
            height += LayoutUtilities.standardControlHeight * 2f;
            height += EditorGUI.GetPropertyHeight(tutorials, true);

            if (type.enumValueIndex == 0)
            {
                height += EditorGUI.GetPropertyHeight(intendedSolution, true);
            }
        }

        return height;
    }
}
