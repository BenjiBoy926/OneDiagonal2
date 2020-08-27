using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Fraction))]
public class FractionPropertyDrawer : PropertyDrawer
{
    const float LABEL_WIDTH = 15f;
    const float CENTER_BUFFER = 5f;

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
        float numberWidth = (position.width - CENTER_BUFFER) / 2f;
        Vector2Int inputs = new Vector2Int();

        GUIStyle slashStyle = new GUIStyle();
        slashStyle.alignment = TextAnchor.MiddleCenter;
        slashStyle.fontStyle = FontStyle.Bold;

        EditorGUIExt.BeginHorizontal(position.position);

        // Put the numerator, slash, denominator
        inputs.x = EditorGUIExt.DelayedIntField(numberWidth, EditorGUIExt.standardControlHeight, numerator.intValue);
        EditorGUIExt.LabelField(CENTER_BUFFER, EditorGUIExt.standardControlHeight, new GUIContent("/"), slashStyle);
        inputs.y = EditorGUIExt.DelayedIntField(numberWidth, EditorGUIExt.standardControlHeight, denominator.intValue);

        EditorGUIExt.EndLayout();

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
