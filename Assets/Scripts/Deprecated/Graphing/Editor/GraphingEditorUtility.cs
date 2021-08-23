using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class GraphingEditorUtility
{
    public static float standardControlHeight
    {
        get
        {
            return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        }
    }
}
