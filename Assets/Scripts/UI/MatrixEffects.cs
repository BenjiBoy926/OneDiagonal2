using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrixEffects
{
    public static void Flash(RectTransform parent)
    {
        GameObject flash = new GameObject("Flash");

        // Add a rect transform to the game object
        RectTransform flashTransform = flash.AddComponent<RectTransform>();
        flashTransform.SetParent(parent);
        flashTransform.SetAsLastSibling();

        // Anchor the flash to stretch over the whole parent
        flashTransform.anchorMin = Vector2.zero;
        flashTransform.anchorMax = Vector2.one;
        flashTransform.pivot = Vector2.one * 0.5f;
        flashTransform.sizeDelta = Vector2.zero;
    }
}
