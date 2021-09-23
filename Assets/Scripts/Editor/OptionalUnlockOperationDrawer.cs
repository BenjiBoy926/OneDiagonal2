using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(OptionalUnlockOperation))]
public class OptionalUnlockOperationDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty willUnlock = property.FindPropertyRelative(nameof(willUnlock));
        SerializedProperty unlocker = property.FindPropertyRelative(nameof(unlocker));
        SerializedProperty operationType = unlocker.FindPropertyRelative(nameof(operationType));
        SerializedProperty unlockSprite = unlocker.FindPropertyRelative(nameof(unlockSprite));
        SerializedProperty unlockItem = unlocker.FindPropertyRelative(nameof(unlockItem));

        // Set height for only one control
        position.height = LayoutUtilities.standardControlHeight;

        // Layout the will unlock property
        EditorGUI.PropertyField(position, willUnlock, label);
        position.y += position.height;

        // If we will unlock then edit the other values
        if(willUnlock.boolValue)
        {
            EditorGUI.indentLevel++;

            // Layout the operation type and sprite
            EditorGUI.PropertyField(position, operationType);
            position.y += position.height;
            EditorGUI.PropertyField(position, unlockSprite);
            position.y += position.height;
            EditorGUI.PropertyField(position, unlockItem);

            EditorGUI.indentLevel--;
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty willUnlock = property.FindPropertyRelative(nameof(willUnlock));
        float height = LayoutUtilities.standardControlHeight;

        if(willUnlock.boolValue)
        {
            height *= 4f;
        }

        return height;
    }
}
