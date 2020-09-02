using UnityEditor;

[CustomEditor(typeof(FractionVariable))]
[CanEditMultipleObjects]
public class FractionVariableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        VariableEditor.OnInspectorGUI(serializedObject);
    }
}
