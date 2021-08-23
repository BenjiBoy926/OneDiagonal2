using UnityEngine;
using System.Collections;

public static class GraphingExtensionMethods
{
    public static bool IsSameAsOrSubclassOf(this System.Type src, System.Type other)
    {
        return src == other || src.IsSubclassOf(other);
    }
}
