using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Result<>))]
public class ResultDrawer : PropertyDrawer
{
    private bool mainFoldout;
    private bool advancedFoldout;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ReferenceDrawer.OnGUI(position, property, label, ReferenceDrawerType.Result, 
            ReferenceDataType.NeitherGameObjectNorComponent);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return ReferenceDrawer.GetPropertyHeight(property, label, ReferenceDrawerType.Result, 
            ReferenceDataType.NeitherGameObjectNorComponent);
    }
}
