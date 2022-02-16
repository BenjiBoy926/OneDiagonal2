using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(LevelCompletionDataList))]
public class LevelCompletionDataListDrawer : PropertyDrawer
{
    #region Property Drawer Overrides
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Setup some useful variables
        SerializedProperty array = property.FindPropertyRelative(nameof(array));
        SerializedProperty levelTypeFilter = property.FindPropertyRelative(nameof(levelTypeFilter));
        string filter = levelTypeFilter.stringValue;

        if (!string.IsNullOrEmpty(filter))
        {
            // Try to parse the string as a level type
            if (System.Enum.TryParse(filter, out LevelType type))
            {
                array.isExpanded = EditorGUIAuto.Foldout(ref position, array.isExpanded, label);

                if (array.isExpanded)
                {
                    // Increase indent
                    EditorGUI.indentLevel++;

                    // Make this array parallel to that array
                    int numLevels = LevelSettings.GetAllLevelDataOfType(type).Length;
                    array.arraySize = numLevels;

                    // Get first array element
                    for (int i = 0; i < array.arraySize; i++)
                    {
                        // Get the element and setup a nice looking label
                        SerializedProperty element = array.GetArrayElementAtIndex(i);
                        LevelID level = new LevelID(type, i);
                        GUIContent elementLabel = new GUIContent(level.Data.EditorDisplayName);

                        // Layout this element
                        EditorGUIAuto.PropertyField(ref position, element, elementLabel, true);
                    }

                    // Restore indent
                    EditorGUI.indentLevel--;
                }
            }
            else EditorGUIAuto.PropertyField(ref position, array, label, true);
        }
        else EditorGUIAuto.PropertyField(ref position, array, label, true);

        // Only display the button if the array is expanded
        if (array.isExpanded)
        {
            // Increase indent
            EditorGUI.indentLevel++;

            // Target state should be true if any are not completed
            SerializedProperty[] elements = EditorGUIAuto.GetArrayElements(array);
            bool targetState = elements
                .Any(elem => !elem.FindPropertyRelative("completed").boolValue);

            // Setup the button label
            string buttonLabel = "Complete all levels";
            if (!targetState) buttonLabel = "Un-complete all levels";

            // Indent the rect for the button
            Rect buttonRect = EditorGUI.IndentedRect(position);

            if (GUIAuto.Button(ref buttonRect, buttonLabel))
            {
                // Go through each element and set it to the target state
                foreach (SerializedProperty elem in elements)
                {
                    SerializedProperty encountered = elem.FindPropertyRelative(nameof(encountered));
                    SerializedProperty completed = elem.FindPropertyRelative(nameof(completed));

                    // Set encountered and completed to the target states
                    encountered.boolValue = targetState;
                    completed.boolValue = targetState;
                }
            }

            // Move the position down by button rect
            position.y = buttonRect.y;

            // Reduce indent
            EditorGUI.indentLevel--;
        }
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // The height of the whole property
        float height;

        // Get the values of relative properties
        SerializedProperty array = property.FindPropertyRelative(nameof(array));
        SerializedProperty levelTypeFilter = property.FindPropertyRelative(nameof(levelTypeFilter));
        string filter = levelTypeFilter.stringValue;

        if (!string.IsNullOrEmpty(filter))
        {
            // Try to parse the string as a level type
            if (System.Enum.TryParse(filter, out LevelType _))
            {
                height = EditorExtensions.StandardControlHeight;

                if (array.isExpanded)
                {
                    // Add up heights for all elements
                    for (int i = 0; i < array.arraySize; i++)
                    {
                        SerializedProperty element = array.GetArrayElementAtIndex(i);
                        height += EditorGUI.GetPropertyHeight(element, true);
                    }
                }
            }
            else height = EditorGUI.GetPropertyHeight(array, true);
        }
        else height = EditorGUI.GetPropertyHeight(array, true);

        if (array.isExpanded)
        {
            // Add space for the special editor button
            height += EditorExtensions.StandardControlHeight;
        }

        return height;
    }
    #endregion
}
