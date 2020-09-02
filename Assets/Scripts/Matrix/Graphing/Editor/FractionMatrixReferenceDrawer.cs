using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(FractionMatrixReference))]
public class FractionMatrixReferenceDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorReferenceDrawer.OnGUI(position, property, label);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorReferenceDrawer.GetPropertyHeight(property, label);
    }
}
