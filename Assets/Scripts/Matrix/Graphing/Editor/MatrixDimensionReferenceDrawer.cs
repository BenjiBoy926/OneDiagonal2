using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(MatrixDimensionReference))]
public class MatrixDimensionReferenceDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ReferenceDrawer.OnGUI(position, property, label);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return ReferenceDrawer.GetPropertyHeight(property, label);
    }
}
