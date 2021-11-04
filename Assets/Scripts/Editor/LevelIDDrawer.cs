using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(LevelID))]
public class LevelIDDrawer : PropertyDrawer
{
    #region Public Methods
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Get sub properties
        SerializedProperty type = property.FindPropertyRelative(nameof(type));
        SerializedProperty index = property.FindPropertyRelative(nameof(index));

        // Set height for just one control
        position.height = LayoutUtilities.standardControlHeight;

        // Put in the property foldout
        property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label);
        position.y += position.height;

        if(property.isExpanded)
        {
            // Increse indent
            EditorGUI.indentLevel++;

            // Edit the type
            EditorGUI.PropertyField(position, type);
            position.y += position.height;

            // Get a list of all possible level ids with this type
            LevelID[] levelIDs = LevelSettings.GetAllLevelIDsOfType((LevelType)type.enumValueIndex);
            // Select the names of the levels with this type
            string[] names = levelIDs.Select(id => id.Data.EditorDisplayName).ToArray();
            // Edit the property as a popup with the names of all the levels in this type
            index.intValue = EditorGUIExt.Popup(position, index.intValue, names, new GUIContent(property.displayName));

            // Resume indent
            EditorGUI.indentLevel--;
        }
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (property.isExpanded)
        {
            return LayoutUtilities.standardControlHeight * 3f;
        }
        else return LayoutUtilities.standardControlHeight;
    }
    #endregion
}
