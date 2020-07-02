using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Fraction))]
public class FractionPropertyDrawer : PropertyDrawer
{
    const float LABEL_WIDTH = 15f;
    const float CENTER_BUFFER = 5f;

    Vector2Int inputs = new Vector2Int();
    Vector2Int simplified = new Vector2Int();

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Store the relative properties
        SerializedProperty numerator = property.FindPropertyRelative("n");
        SerializedProperty denominator = property.FindPropertyRelative("d");

        // Put in the prefix
        Rect newPosition = EditorGUI.PrefixLabel(position, label);

        // Calculate the width of each number input
        float numberWidth = (newPosition.width - (2 * LABEL_WIDTH) - CENTER_BUFFER) / 2f;

        // Calculate the rect area of each sub control
        Rect numeratorLabelRect = new Rect(newPosition.x, newPosition.y, LABEL_WIDTH, newPosition.height);
        Rect numeratorRect = new Rect(newPosition.x + LABEL_WIDTH, newPosition.y, numberWidth, newPosition.height);
        Rect denominatorLabelRect = new Rect(newPosition.x + LABEL_WIDTH + numberWidth + CENTER_BUFFER, newPosition.y, LABEL_WIDTH, newPosition.height);
        Rect denominatorRect = new Rect(newPosition.x + (2 * LABEL_WIDTH) + numberWidth + CENTER_BUFFER, newPosition.y, numberWidth, newPosition.height);

        EditorGUI.BeginChangeCheck();

        // Put the label and the numerator input
        EditorGUI.LabelField(numeratorLabelRect, new GUIContent("N"));
        inputs.x = EditorGUI.DelayedIntField(numeratorRect, numerator.intValue);

        // Put the label and the denominator input
        EditorGUI.LabelField(denominatorLabelRect, new GUIContent("D"));
        inputs.y = EditorGUI.DelayedIntField(denominatorRect, denominator.intValue);

        if(EditorGUI.EndChangeCheck())
        {
            // Simplify the numbers input
            simplified = MyMath.Simplify(inputs);

            // Assign the simplified values back into the fraction
            numerator.intValue = simplified.x;
            denominator.intValue = simplified.y;
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label);
    }
}
