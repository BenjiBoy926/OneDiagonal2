using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILine : MonoBehaviour
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("List of segments that make up the line")]
    private List<UILineSegment> segments;
    #endregion

    #region Public Methods
    public void SetPoints(params Vector3[] points)
    {
        Redraw(points);
    }
    public void Redraw(params Vector3[] points)
    {
        // There should always be one more point than there are line segments
        if (segments.Count == points.Length - 1)
        {
            // Set the points for each line segment
            for(int i = 0; i < points.Length - 1; i++)
            {
                segments[i].SetPoints(points[i], points[i + 1]);
            }
        }
        else Debug.LogWarning($"{nameof(UILine)}: cannot redraw ui line with " +
            $"{segments.Count} segments using {points.Length} points, " +
            $"exactly {segments.Count + 1} points expected");
    }
    #endregion
}
