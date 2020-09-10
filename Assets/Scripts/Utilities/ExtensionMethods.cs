using System.Collections.Generic;
using UnityEditor;

public static class ExtensionMethods
{
    public static IEnumerable<SerializedProperty> GetChildren(this SerializedProperty serializedProperty)
    {
        SerializedProperty currentProperty = serializedProperty.Copy();
        SerializedProperty nextSiblingProperty = serializedProperty.Copy();
        {
            nextSiblingProperty.Next(false);
        }

        if (currentProperty.Next(true))
        {
            do
            {
                if (SerializedProperty.EqualContents(currentProperty, nextSiblingProperty))
                    break;

                yield return currentProperty;
            }
            while (currentProperty.Next(false));
        }
    }
}
