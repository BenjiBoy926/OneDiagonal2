using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UILineSegment)), CanEditMultipleObjects]
public class UILineSegmentEditor : Editor
{
    private DrivenRectTransformTracker tracker = new DrivenRectTransformTracker();

    public override void OnInspectorGUI()
    {
        // Get the target as the script itself
        UILineSegment targetScript = (UILineSegment)target;
        // Create a tracker to drive the aspects of the rect transform
        tracker.Add(target, targetScript.RectTransform,
            DrivenTransformProperties.AnchoredPosition |
            DrivenTransformProperties.SizeDelta |
            DrivenTransformProperties.Anchors |
            DrivenTransformProperties.Pivot |
            DrivenTransformProperties.Rotation
        );

        // Draw default inspector and check for changes
        EditorGUI.BeginChangeCheck();
        DrawDefaultInspector();

        // Redraw the line if something changed
        if(EditorGUI.EndChangeCheck())
        {
            targetScript.Redraw();
        }
    }
}
