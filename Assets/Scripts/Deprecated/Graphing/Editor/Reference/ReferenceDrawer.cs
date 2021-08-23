using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public static class ReferenceDrawer
{
    public const float TYPE_WIDTH = 80f;
   
    public static void OnGUI(Rect position, SerializedProperty property, GUIContent label, 
        ReferenceDrawerType drawerType, ReferenceDataType dataType)
    {
        SerializedProperty mainFoldout = property.FindPropertyRelative("mainFoldout");
        SerializedProperty advancedFoldout = property.FindPropertyRelative("advancedFoldout");

        // Get starting rect
        Rect rect = new Rect(position.position, new Vector2(position.width, GraphingEditorUtility.standardControlHeight));

        // Put the foldout item
        mainFoldout.boolValue = EditorGUI.Foldout(rect, mainFoldout.boolValue, label);
        rect.y += GraphingEditorUtility.standardControlHeight;

        if (mainFoldout.boolValue)
        {
            // Put the evaluation type
            EditorGUI.indentLevel++;
            rect = OnGUIEvaluationType(rect, property, drawerType);

            // Put the reference type
            rect = OnGUIReferenceType(rect, property, drawerType);

            switch(property.FindPropertyRelative("referenceType").enumValueIndex)
            {
                // GUI for a direct value
                case 0:
                    rect = OnGUIDirectValue(rect, property, drawerType);
                    break;
                // GUI for the indirect value
                case 1:
                    rect = OnGUIIndirectValue(rect, property, drawerType);
                    break;
                // GUI for a redirected value
                case 2:
                    // Display redirected value options
                    rect = OnGUIRedirectedValue(rect, property, drawerType);

                    // Display advanced options
                    rect = OnGUIAdvancedOptions(rect, property, drawerType, dataType);

                    break;
            }

            EditorGUI.indentLevel--;
        }
    }

    public static float GetPropertyHeight(SerializedProperty property, GUIContent content,  
        ReferenceDrawerType drawerType, ReferenceDataType dataType)
    {
        // Get foldout values
        bool mainFoldout = property.FindPropertyRelative("mainFoldout").boolValue;
        bool advancedFoldout = property.FindPropertyRelative("advancedFoldout").boolValue;

        // Start with the main foldout line
        float current = GraphingEditorUtility.standardControlHeight;

        if (mainFoldout)
        {
            SerializedProperty evaluationType = property.FindPropertyRelative("evaluationType");
            SerializedProperty referenceType = property.FindPropertyRelative("referenceType");

            // If this is an input, account for the evaluation type
            if(drawerType == ReferenceDrawerType.Input)
            {
                current += GraphingEditorUtility.standardControlHeight;
            }

            // Account for reference type height
            current += GraphingEditorUtility.standardControlHeight;

            // Account for value height
            if (referenceType.enumValueIndex == 0 && evaluationType.enumValueIndex == 0)
            {
                current += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("directValue"));
            }
            else current += GraphingEditorUtility.standardControlHeight;

            // If reference is a redirect, account for game object redirect type and advanced foldout
            if(referenceType.enumValueIndex == 2)
            {
                current += 2f * GraphingEditorUtility.standardControlHeight;

                // If advanced options are opened up, account for redirect depth
                if(advancedFoldout)
                {
                    current += GraphingEditorUtility.standardControlHeight;

                    // If reference drawer is for input and a GameObject or Component,
                    // account for redirect type height
                    if(drawerType == ReferenceDrawerType.Input && dataType == ReferenceDataType.GameObjectOrComponent)
                    {
                        current += GraphingEditorUtility.standardControlHeight;
                    }
                }
            }
        }

        return current;
    }

    private static Rect OnGUIEvaluationType(Rect content, SerializedProperty property, ReferenceDrawerType drawerType)
    {
        if(drawerType == ReferenceDrawerType.Input)
        {
            EditorGUI.PropertyField(content, property.FindPropertyRelative("evaluationType"));
            content.y += GraphingEditorUtility.standardControlHeight;
        }
        return content;
    }

    private static Rect OnGUIReferenceType(Rect content, SerializedProperty property, ReferenceDrawerType drawerType)
    {
        // Calculate the content for the reference button toggle and the reference drawer
        SerializedProperty referenceType = property.FindPropertyRelative("referenceType");

        // If not an input type, display property as normal
        if (drawerType == ReferenceDrawerType.Input)
        {
            EditorGUI.PropertyField(content, referenceType);
        }
        // If this is a result, remove the first enum option
        else
        {
            int newValue = referenceType.enumValueIndex - 1;
            if (newValue < 0)
            {
                newValue = 0;
            }

            List<string> enumOptions = new List<string>(referenceType.enumDisplayNames);

            // Put in a popup with the first enum option removed
            enumOptions.RemoveAt(0);
            newValue = EditorGUI.Popup(content, "Reference Type", newValue, enumOptions.ToArray());

            referenceType.enumValueIndex = newValue + 1;
        }

        return new Rect(content.x, content.y + GraphingEditorUtility.standardControlHeight, 
            content.width, content.height);
    }

    private static Rect OnGUIDirectValue(Rect content, SerializedProperty property, ReferenceDrawerType drawerType)
    {
        if(drawerType == ReferenceDrawerType.Input)
        {
            SerializedProperty evaluationType = property.FindPropertyRelative("evaluationType");

            switch(evaluationType.enumValueIndex)
            {
                case 0:
                    SerializedProperty directValue = property.FindPropertyRelative("directValue");
                    EditorGUI.PropertyField(content, directValue, true);
                    content.y += EditorGUI.GetPropertyHeight(directValue);
                    break;
                case 1:
                    EditorGUI.PropertyField(content, property.FindPropertyRelative("directAction"));
                    content.y += GraphingEditorUtility.standardControlHeight;
                    break;
            }
        }

        return content;
    }

    private static Rect OnGUIIndirectValue(Rect content, SerializedProperty property, ReferenceDrawerType drawerType)
    {
        if(drawerType == ReferenceDrawerType.Input)
        {
            SerializedProperty evaluationType = property.FindPropertyRelative("evaluationType");

            switch (evaluationType.enumValueIndex)
            {
                case 0:
                    EditorGUI.PropertyField(content, property.FindPropertyRelative("indirectValue"));
                    content.y += GraphingEditorUtility.standardControlHeight;
                    break;
                case 1:
                    EditorGUI.PropertyField(content, property.FindPropertyRelative("indirectAction"));
                    content.y += GraphingEditorUtility.standardControlHeight;
                    break;
            }
        }
        else
        {
            EditorGUI.PropertyField(content, property.FindPropertyRelative("indirectValue"));
            content.y += GraphingEditorUtility.standardControlHeight;
        }

        return content;
    }

    private static Rect OnGUIRedirectedValue(Rect content, SerializedProperty property, ReferenceDrawerType drawerType)
    {
        SerializedProperty gameObjectReferenceType = property.FindPropertyRelative("gameObjectReferenceType");

        // Display game object reference
        EditorGUI.PropertyField(content, gameObjectReferenceType);
        content.y += GraphingEditorUtility.standardControlHeight;

        // Display drawer for game object variable or tag
        if (gameObjectReferenceType.enumValueIndex == 0)
        {
            EditorGUI.PropertyField(content, property.FindPropertyRelative("redirectVariable"));
        }
        else EditorGUI.PropertyField(content, property.FindPropertyRelative("redirectAction"));
        content.y += GraphingEditorUtility.standardControlHeight;

        return content;
    }

    private static Rect OnGUIAdvancedOptions(Rect current, SerializedProperty property, 
        ReferenceDrawerType drawerType, ReferenceDataType dataType)
    {
        SerializedProperty advancedFoldout = property.FindPropertyRelative("advancedFoldout");
        advancedFoldout.boolValue = EditorGUI.Foldout(current, advancedFoldout.boolValue, new GUIContent("Advanced"));
        current.y += GraphingEditorUtility.standardControlHeight;

        if(advancedFoldout.boolValue)
        {
            EditorGUI.indentLevel++;

            // If drawer is input, display redirect type
            if(drawerType == ReferenceDrawerType.Input && dataType == ReferenceDataType.GameObjectOrComponent)
            {
                EditorGUI.PropertyField(current, property.FindPropertyRelative("redirectType"));
                current.y += GraphingEditorUtility.standardControlHeight;
            }

            // Display redirect depth
            EditorGUI.PropertyField(current, property.FindPropertyRelative("redirectDepth"));
            current.y += GraphingEditorUtility.standardControlHeight;

            EditorGUI.indentLevel--;
        }

        return current;
    }
}
