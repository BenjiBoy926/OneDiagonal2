using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Fraction))]
public class FractionPropertyDrawer : PropertyDrawer
{
    const float CENTER_BUFFER = 15f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Put in the prefix
        Rect newPosition = EditorGUI.PrefixLabel(position, label);

        // Check for changes in fraction
        EditorGUI.BeginChangeCheck();

        // Put in fraction GUI
        Vector2Int inputs = OnGUIFraction(newPosition, property);

        // If fraction changes, simplify it
        if(EditorGUI.EndChangeCheck())
        {
            SimplifyFraction(inputs, property);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label);
    }

    private Vector2Int OnGUIFraction(Rect position, SerializedProperty property)
    {
        SerializedProperty numerator = property.FindPropertyRelative("n");
        SerializedProperty denominator = property.FindPropertyRelative("d");
        Vector2Int inputs = new Vector2Int();

        GUIStyle slashStyle = new GUIStyle(EditorStyles.boldLabel);
        slashStyle.alignment = TextAnchor.MiddleCenter;
        slashStyle.fontStyle = FontStyle.Bold;

        Layout.Builder builder = new Layout.Builder();
        builder.PushChild(new LayoutChild(LayoutSize.RatioOfRemainder(0.5f)));
        builder.PushChild(new LayoutChild(LayoutSize.Exact(CENTER_BUFFER)));
        builder.PushChild(new LayoutChild(LayoutSize.RatioOfRemainder(0.5f)));
        Layout layout = builder.Compile(position);

        // Reset to no indent
        int oldIndent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Put the numerator, slash, denominator
        inputs.x = EditorGUI.DelayedIntField(layout.Next(), numerator.intValue);
        EditorGUI.LabelField(layout.Next(), new GUIContent("/"), slashStyle);
        inputs.y = EditorGUI.DelayedIntField(layout.Next(), denominator.intValue);

        // Restore old indent
        EditorGUI.indentLevel = oldIndent;

        return inputs;
    }

    private void SimplifyFraction(Vector2Int inputs, SerializedProperty property)
    {
        SerializedProperty numerator = property.FindPropertyRelative("n");
        SerializedProperty denominator = property.FindPropertyRelative("d");

        // Simplify the numbers input
        Vector2Int simplified = MyMath.FractionSimplify(inputs);

        // Assign the simplified values back into the fraction
        numerator.intValue = simplified.x;
        denominator.intValue = simplified.y;
    }
}
