using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(TutorialData))]
public class TutorialDataDrawer : PropertyDrawer
{
    #region Property Drawer Overrides
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Display the foldout
        property.isExpanded = EditorGUIAuto.Foldout(ref position, property.isExpanded, label);

        if (property.isExpanded)
        {
            // Increase indent
            EditorGUI.indentLevel++;

            // Get enumerable list of children
            IEnumerable<SerializedProperty> children = EditorGUIAuto.ToEnd(
                property, "optionalUnlockData", "explanation", false, false);
            SerializedProperty visualType = property.FindPropertyRelative(nameof(visualType));
        
            // Layout every child
            foreach(SerializedProperty child in children)
            {
                // Only layout sprite if enum value is 0
                if (child.name == "sprite" && visualType.enumValueIndex == 0)
                {
                    EditorGUI.indentLevel++;
                    EditorGUIAuto.PropertyField(ref position, child, true);
                    EditorGUI.indentLevel--;
                }
                // Only layout video sub path if enum value is 1
                else if (child.name == "videoStreamingSubPath" && visualType.enumValueIndex == 1)
                {
                    EditorGUI.indentLevel++;
                    EditorGUIAuto.PropertyField(ref position, child, true);
                    EditorGUI.indentLevel--;
                }
                // No special layout rules for other properties
                else if (child.name != "sprite" && child.name != "videoStreamingSubPath")
                {
                    EditorGUIAuto.PropertyField(ref position, child, true);
                }
            }

            // Reduce indent
            EditorGUI.indentLevel--;
        }
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float height = EditorGUI.GetPropertyHeight(property, label, true);
        if (property.isExpanded) height -= EditorGUIAuto.SingleControlHeight;
        return height;
    }
    #endregion
}
