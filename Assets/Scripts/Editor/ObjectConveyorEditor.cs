using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ObjectConveyor))]
public class ObjectConveyorEditor : Editor
{
    #region Editor Overrides
    public override void OnInspectorGUI()
    {
        // Update the object
        serializedObject.Update();

        // Get the end point type
        SerializedProperty endPointType = serializedObject.FindProperty(nameof(endPointType));

        // Get the first and last properties
        SerializedProperty start = serializedObject.FindProperty("localStartPoint");
        SerializedProperty end = serializedObject
            .FindProperty("conveyorSpeed")
            .GetEndProperty();

        // Get all the properties in the object
        IEnumerable<SerializedProperty> properties = EditorGUIAuto.ToEnd(start, end, false, false);

        // Show the script referenced by the object
        GUI.enabled = false;
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
        GUI.enabled = true;
    
        // Go through each property in the list of properties
        foreach (SerializedProperty property in properties)
        {
            // Check if the end point changes
            if (property.name == "endPointType")
            {
                EditorGUI.BeginChangeCheck();
            }

            // If end point is implicit then disable editing the end point
            if (endPointType.enumValueIndex == 0 && property.name == "localEndPoint")
            {
                GUI.enabled = false;
            }
            // If end point is explicit then disable editing the direction and offset
            if (endPointType.enumValueIndex == 1 && 
                (property.name == "direction" || 
                property.name == "objectOffset"))
            {
                GUI.enabled = false;
            }

            // Display the property
            EditorGUILayout.PropertyField(property, true);
            GUI.enabled = true;

            // Check if the end point type changed
            if (property.name == "endPointType")
            {
                if (EditorGUI.EndChangeCheck())
                {
                    // Apply the modified end point type
                    serializedObject.ApplyModifiedProperties();

                    // Get the object targetted by the editor
                    ObjectConveyor conveyor = target as ObjectConveyor;

                    // If the end point is implicit,
                    // then compute it and show it in the editor
                    if (property.enumValueIndex == 0)
                    {
                        SerializedProperty localEndPoint = serializedObject.FindProperty(nameof(localEndPoint));
                        localEndPoint.vector3Value = conveyor.LocalEndPoint;
                    }
                    // If the end point is explicit,
                    // then compute direction and offset to show in editor
                    else
                    {
                        SerializedProperty direction = serializedObject.FindProperty(nameof(direction));
                        direction.vector3Value = conveyor.Direction;

                        SerializedProperty objectOffset = serializedObject.FindProperty(nameof(objectOffset));
                        objectOffset.floatValue = conveyor.ObjectOffset;
                    }

                    // Update the serialized object to reflect the new values
                    serializedObject.Update();
                }
            }
        }

        // Apply any modified properties
        serializedObject.ApplyModifiedProperties();
    }
    #endregion
}
