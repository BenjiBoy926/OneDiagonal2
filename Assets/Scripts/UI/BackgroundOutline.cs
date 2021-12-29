using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundOutline : MonoBehaviour
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Rect transform to use for the background outline")]
    private RectTransform rectTransform;
    [SerializeField]
    [Tooltip("Thickness of the outline")]
    private float thickness;
    #endregion

    #region Public Methods
    public void Outline(RectTransform target)
    {

    }
    #endregion
}
