using UnityEngine;

public struct EditorGUILayoutData
{
    // FIELDS
    public Vector2 position { get; private set; }
    public RectTransform.Axis type { get; private set; }

    // Largest element in the layout on the main axis (width or height)
    public float maxMainAxisDimension { get; private set; }

    // CONSTRUCTORS
    public EditorGUILayoutData(Vector2 newPosition, RectTransform.Axis newType) : this(newPosition, newType, 0) { }
    private EditorGUILayoutData(EditorGUILayoutData other) : this(other.position, other.type, other.maxMainAxisDimension) { }
    private EditorGUILayoutData(Vector2 newPosition, RectTransform.Axis newType, float newMaxMainAxisDimension)
    {
        position = newPosition;
        type = newType;
        maxMainAxisDimension = newMaxMainAxisDimension;
    }

    public EditorGUILayoutData AddGUIElement(float length)
    {
        return AddGUIElement(length, length);
    }

    // Update the layout data by adding an item with the given width-height
    public EditorGUILayoutData AddGUIElement(float width, float height)
    {
        EditorGUILayoutData newData = new EditorGUILayoutData(this);

        switch (type)
        {
            case RectTransform.Axis.Horizontal:
                newData.position = new Vector2(newData.position.x + width, newData.position.y);
                if (newData.maxMainAxisDimension < width) newData.maxMainAxisDimension = width;
                break;
            case RectTransform.Axis.Vertical:
                newData.position = new Vector2(newData.position.x, newData.position.y + height);
                if (newData.maxMainAxisDimension < height) newData.maxMainAxisDimension = height;
                break;
        }

        return newData;
    }
}
