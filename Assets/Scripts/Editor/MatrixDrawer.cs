using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Matrix))]
[CanEditMultipleObjects]
public class MatrixDrawer : PropertyDrawer
{
    // CONSTANTS
    const float EDGE_BUFFER = 15f;
    const float ROWS_LABEL_WIDTH = 40f;
    const float COLS_LABEL_WIDTH = 32f;
    const float DIMENSION_CENTER_BUFFER = 15f;

    const float ITEM_LABEL_WIDTH = 35f;
    const float HORIZONTAL_ITEM_SPACE = 15f;

    // FIELDS
    private int cols;

    private void SetCols(SerializedProperty property)
    {
        int rows = property.FindPropertyRelative("_rows").intValue;
        int arraySize = property.FindPropertyRelative("data").arraySize;
        cols = arraySize / rows;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        int rows = property.FindPropertyRelative("_rows").intValue;
        SetCols(property);

        Layout.Builder builder = new Layout.Builder();
        builder.Orientation(LayoutOrientation.Vertical);
        builder.PushChild(new LayoutChild(LayoutMargin.Bottom()));
        builder.PushChild(new LayoutChild(LayoutMargin.Bottom()));
        for(int i = -1; i < rows; i++)
        {
            builder.PushChild(new LayoutChild(LayoutMargin.Bottom()));
        }
        Layout layout = builder.Compile(position);

        property.isExpanded = EditorGUI.Foldout(layout.Next(), property.isExpanded, label);

        if(property.isExpanded)
        {
            EditorGUI.indentLevel++;

            // Check for change, and add GUI for adjusting rows and cols
            EditorGUI.BeginChangeCheck();
            OnGUIRowsAndCols(layout, property);

            // If rows or columns change, adjust the property's data array
            if(EditorGUI.EndChangeCheck())
            {
                AdjustPropertyArray(property);
            }

            // If the matrix has some rows and columns, render it
            if(rows > 0 && cols > 0)
            {
                OnGUIMatrix(layout, property);
            }

            EditorGUI.indentLevel--;
        }

        EditorGUIExt.EndLayout();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        int rows = property.FindPropertyRelative("_rows").intValue;

        if (!property.isExpanded) return EditorGUIExt.standardControlHeight;
        else if (rows <= 0 || cols <= 0) return EditorGUIExt.standardControlHeight * 2f;
        else return EditorGUIExt.standardControlHeight * (rows + 3);
    }

    private void OnGUIRowsAndCols(Layout verticalLayout, SerializedProperty property)
    {
        SerializedProperty rows = property.FindPropertyRelative("_rows");

        Layout.Builder builder = new Layout.Builder();
        builder.PushChild(new LayoutChild(LayoutSize.Exact(ROWS_LABEL_WIDTH)));
        builder.PushChild(new LayoutChild(LayoutSize.RatioOfRemainder(0.5f), LayoutMargin.Right(DIMENSION_CENTER_BUFFER)));
        builder.PushChild(new LayoutChild(LayoutSize.Exact(COLS_LABEL_WIDTH)));
        builder.PushChild(new LayoutChild(LayoutSize.RatioOfRemainder(0.5f)));
        Layout layout = builder.Compile(EditorGUI.IndentedRect(verticalLayout.Next()));

        EditorGUIExt.PushIndent(0);

        // Add label and int for row
        EditorGUI.LabelField(layout.Next(), new GUIContent("Rows:"));
        rows.intValue = EditorGUI.DelayedIntField(layout.Next(), rows.intValue);

        // Add label and int for columns
        EditorGUI.LabelField(layout.Next(), new GUIContent("Cols:"));
        cols = EditorGUI.DelayedIntField(layout.Next(), cols);

        EditorGUIExt.PopIndent();
    }

    private void AdjustPropertyArray(SerializedProperty property)
    {
        SerializedProperty data = property.FindPropertyRelative("data");
        int rows = property.FindPropertyRelative("_rows").intValue;

        int oldArraySize = data.arraySize;
        int newArraySize = rows * cols;

        data.arraySize += newArraySize - oldArraySize;
    }

    private void OnGUIMatrix(Layout verticalLayout, SerializedProperty property)
    {
        SerializedProperty data = property.FindPropertyRelative("data");
        int rows = property.FindPropertyRelative("_rows").intValue;

        // The style for a label of the row, column
        GUIStyle labelStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            alignment = TextAnchor.MiddleCenter
        };

        for (int i = -1; i < rows; i++)
        {
            // Build the horizontal layout that will be in the current part of the vertical layout
            Layout.Builder builder = new Layout.Builder();
            builder.PushChild(new LayoutChild(LayoutSize.Exact(ITEM_LABEL_WIDTH)));
            for(int j = 0; j < cols; j++)
            {
                builder.PushChild(new LayoutChild(LayoutSize.RatioOfRemainder(1f / cols), LayoutMargin.Right(HORIZONTAL_ITEM_SPACE)));
            }
            Layout horizontalLayout = builder.Compile(EditorGUI.IndentedRect(verticalLayout.Next()));

            // No indent because it is accounted for when the builder built the rect in the indented rect
            EditorGUIExt.PushIndent(0);

            for(int j = -1; j < cols; j++)
            {
                // Code inside is for the row with the labels
                if(i == -1)
                {
                    // If we're at the top corner, put "[X, Y]"
                    if(j == -1)
                    {
                        EditorGUI.LabelField(horizontalLayout.Next(), new GUIContent("[X, Y]"), labelStyle);
                    }
                    // If we're in the top row, put down the column numbers
                    else
                    {
                        EditorGUI.LabelField(horizontalLayout.Next(), new GUIContent(j.ToString()), labelStyle);
                    }
                }
                // Code inside is for the column with the labels
                else if(j == -1)
                {
                    EditorGUI.LabelField(horizontalLayout.Next(), new GUIContent(i.ToString()), labelStyle);
                }
                // Code in here displays the property
                else
                {
                    EditorGUI.PropertyField(horizontalLayout.Next(), data.GetArrayElementAtIndex(MyMath.Index2Dto1D(i, j, cols)), GUIContent.none);
                }
            }
        }

        EditorGUIExt.PopIndent();
    }
}
