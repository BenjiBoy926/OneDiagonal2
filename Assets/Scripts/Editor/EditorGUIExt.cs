using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public static class EditorGUIExt
{
    // FIELDS
    private static List<EditorGUILayoutData> layout = new List<EditorGUILayoutData>();
    private static Stack<int> indentStack = new Stack<int>();

    // PROPERTIES
    public static float standardControlHeight => EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
    public static bool layoutActive => layout.Count > 0;
    public static Vector2 currentPosition => layoutActive ? layout[layout.Count - 1].position : Vector2.zero;

    // 
    public static void PushIndent(int newIndent)
    {
        indentStack.Push(EditorGUI.indentLevel);
        EditorGUI.indentLevel = newIndent;
    }
    public static void PopIndent()
    {
        EditorGUI.indentLevel = indentStack.Pop();
    }

    // Begin layout
    private static void BeginLayout(Vector2 position, RectTransform.Axis axis)
    {
        layout.Add(new EditorGUILayoutData(position, axis));
    }
    public static void BeginHorizontal(Vector2 position)
    {
        BeginLayout(position, RectTransform.Axis.Horizontal);
    }
    public static void BeginVertical(Vector2 position)
    {
        BeginLayout(position, RectTransform.Axis.Vertical);
    }
    // End layout
    public static void EndLayout()
    {
        if(layoutActive)
        {
            EditorGUILayoutData data = layout[layout.Count - 1];
            layout.RemoveAt(layout.Count - 1);

            if(layoutActive)
            {
                layout[layout.Count - 1] = layout[layout.Count - 1].AddGUIElement(data.maxMainAxisDimension);
            }
        }
    }
    // Add a space in the layout
    public static void Space(float space)
    {
        AddGUIElement(space);
    }

    // DELEGATES - versions of functions on EditorGUI that use the current layout position
    public static void LabelField(float width, float height, GUIContent label)
    {
        EditorGUI.LabelField(CurrentRect(width, height), label);
        AddGUIElement(width, height);
    }
    public static void LabelField(float width, float height, GUIContent label, GUIStyle style)
    {
        EditorGUI.LabelField(CurrentRect(width, height), label, style);
        AddGUIElement(width, height);
    }
    public static int DelayedIntField(float width, float height, int value)
    {
        int newValue = EditorGUI.DelayedIntField(CurrentRect(width, height), value);
        AddGUIElement(width, height);
        return newValue;
    }
    public static bool Foldout(float width, float height, bool foldout, GUIContent label)
    {
        bool newFoldout = EditorGUI.Foldout(CurrentRect(width, height), foldout, label);
        AddGUIElement(width, height);
        return newFoldout;
    }
    public static bool PropertyField(float width, float height, SerializedProperty property, GUIContent label)
    {
        bool value = EditorGUI.PropertyField(CurrentRect(width, height), property, label);
        AddGUIElement(width, height);
        return value;
    }

    private static Rect CurrentRect(float width, float height)
    {
        if (layoutActive)
        {
            Vector2 position = layout[layout.Count - 1].position;
            return new Rect(position.x, position.y, width, height);
        }
        else return new Rect();
    }
    private static void AddGUIElement(float length)
    {
        AddGUIElement(length, length);
    }
    private static void AddGUIElement(float width, float height)
    {
        if(layoutActive)
        {
            layout[layout.Count - 1] = layout[layout.Count - 1].AddGUIElement(width, height);
        }
    }
}
