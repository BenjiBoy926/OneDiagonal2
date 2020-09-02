using UnityEditor;

[CustomEditor(typeof(FractionMatrixVariable))]
[CanEditMultipleObjects]
public class FractionMatrixVariableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        VariableEditor.OnInspectorGUI(serializedObject);
    }
}
