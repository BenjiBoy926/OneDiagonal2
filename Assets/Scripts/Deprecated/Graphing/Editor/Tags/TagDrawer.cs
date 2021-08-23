using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Tag))]
public class TagDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty value = property.FindPropertyRelative("value");
        value.stringValue = EditorGUI.TagField(position, label, value.stringValue);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return GraphingEditorUtility.standardControlHeight;
    }
}