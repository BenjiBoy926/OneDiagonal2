using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(FractionMatrix))]
[CanEditMultipleObjects]
public class FractionMatrixDrawer : PropertyDrawer
{
    // CONSTANTS
    const float EDGE_BUFFER = 15f;
    const float ROWS_LABEL_WIDTH = 40f;
    const float COLS_LABEL_WIDTH = 32f;
    const float DIMENSION_CENTER_BUFFER = 15f;
    const float ITEM_LABEL_WIDTH = 35f;
    const float HORIZONTAL_ITEM_SPACE = 15f;

    // FIELDS
    private bool foldout;
    private int cols = 1;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        int rows = property.FindPropertyRelative("rows").intValue;

        Layout.Builder builder = new Layout.Builder();
        builder.Orientation(LayoutOrientation.Vertical);
        builder.PushChild(new LayoutChild(LayoutMargin.Bottom()));
        builder.PushChild(new LayoutChild(LayoutMargin.Bottom()));

        for(int i = -1; i < rows; i++)
        {
            builder.PushChild(new LayoutChild(LayoutMargin.Bottom()));
        }

        Layout layout = builder.Compile(position);

        foldout = EditorGUI.Foldout(layout.Next(), foldout, label);

        if(foldout)
        {
            // Check for change, and add GUI for adjusting rows and cols
            EditorGUI.BeginChangeCheck();
            OnGUIRowsAndCols(layout, property);

            // If rows or columns change, adjust the property's data array
            if(EditorGUI.EndChangeCheck())
            {
                AdjustPropertyArray(property);
            }

            OnGUIMatrix(layout, property);
        }

        EditorGUIExt.EndLayout();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (!foldout) return EditorGUIExt.standardControlHeight;
        else
        {
            int rows = property.FindPropertyRelative("rows").intValue;
            return EditorGUIExt.standardControlHeight * (rows + 3);
        }
    }

    private void OnGUIRowsAndCols(Layout verticalLayout, SerializedProperty property)
    {
        SerializedProperty rows = property.FindPropertyRelative("rows");

        Layout.Builder builder = new Layout.Builder();
        builder.PushChild(new LayoutChild(LayoutSize.Exact(ROWS_LABEL_WIDTH)));
        builder.PushChild(new LayoutChild(LayoutSize.RatioOfRemainder(0.5f), LayoutMargin.Right(DIMENSION_CENTER_BUFFER)));
        builder.PushChild(new LayoutChild(LayoutSize.Exact(COLS_LABEL_WIDTH)));
        builder.PushChild(new LayoutChild(LayoutSize.RatioOfRemainder(0.5f)));
        Layout layout = builder.Compile(verticalLayout.Next());

        // Add label and int for row
        EditorGUI.LabelField(layout.Next(), new GUIContent("Rows:"));
        rows.intValue = EditorGUI.DelayedIntField(layout.Next(), rows.intValue);

        // Add label and int for columns
        EditorGUI.LabelField(layout.Next(), new GUIContent("Cols:"));
        cols = EditorGUI.DelayedIntField(layout.Next(), cols);
    }

    private void AdjustPropertyArray(SerializedProperty property)
    {
        SerializedProperty data = property.FindPropertyRelative("data");
        int rows = property.FindPropertyRelative("rows").intValue;
        int oldArraySize = data.arraySize;
        
        data.arraySize = rows * cols;
        
        // Add elements onto the end of the array as needed
        for(int i = oldArraySize; i < data.arraySize; i++)
        {
            data.InsertArrayElementAtIndex(i);
        }
    }

    private void OnGUIMatrix(Layout verticalLayout, SerializedProperty property)
    {
        SerializedProperty data = property.FindPropertyRelative("data");
        
        int rows = property.FindPropertyRelative("rows").intValue;

        //float itemWidth = (width - ITEM_LABEL_WIDTH - ((cols - 1) * HORIZONTAL_ITEM_SPACE)) / cols;

        GUIStyle labelStyle = new GUIStyle();
        labelStyle.alignment = TextAnchor.MiddleCenter;
        labelStyle.fontStyle = FontStyle.Bold;

        for(int i = -1; i < rows; i++)
        {
            EditorGUIExt.BeginHorizontal(EditorGUIExt.currentPosition);

            for(int j = -1; j < cols; j++)
            {
                // Code inside is for the row with the labels
                if(i == -1)
                {
                    // If we're at the top corner, put "[X, Y]"
                    if(j == -1)
                    {
                        EditorGUIExt.LabelField(ITEM_LABEL_WIDTH, EditorGUIExt.standardControlHeight, new GUIContent("[X, Y]"), labelStyle);
                    }
                    // If we're in the top row, put down the column numbers
                    else
                    {
                        //EditorGUIExt.LabelField(itemWidth, EditorGUIExt.standardControlHeight, new GUIContent(j.ToString()), labelStyle);
                        EditorGUIExt.Space(HORIZONTAL_ITEM_SPACE);
                    }
                }
                // Code inside is for the column with the labels
                else if(j == -1)
                {
                    EditorGUIExt.LabelField(ITEM_LABEL_WIDTH, EditorGUIExt.standardControlHeight, new GUIContent(i.ToString()), labelStyle);
                }
                // Code in here displays the property
                else
                {
                    //EditorGUIExt.PropertyField(itemWidth, EditorGUIExt.standardControlHeight, data.GetArrayElementAtIndex(MyMath.Index(i, j, rows, cols)), GUIContent.none);
                    EditorGUIExt.Space(HORIZONTAL_ITEM_SPACE);
                }
            }

            EditorGUIExt.EndLayout();
        }
    }
}
