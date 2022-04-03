using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutlineEffect : MonoBehaviour
{
    #region Public Properties
    public Image Image => image;
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Reference to the image used for this outline")]
    private Image image;
    [SerializeField]
    [Tooltip("Offset of the outline from the parent rect transform")]
    private Vector2 offset = new Vector2(5, 5);
    [SerializeField]
    [Tooltip("Scale of the outline to fade in from and out to")]
    private float fadeOutScale = 2f;
    [SerializeField]
    [Tooltip("Time it takes for the effect to fade in and out")]
    private float fadeTime = 0.2f;
    #endregion

    #region Private Fields
    private DrivenRectTransformTracker rectTransformTracker = new DrivenRectTransformTracker();
    #endregion

    #region Public Methods
    public void UpdateUI()
    {
        // Get my rect trasform
        RectTransform rectTransform = transform as RectTransform;

        // Drive some properties on the rect transform
        rectTransformTracker.Clear();
        rectTransformTracker.Add(this, rectTransform, 
            DrivenTransformProperties.AnchoredPosition | 
            DrivenTransformProperties.SizeDelta | 
            DrivenTransformProperties.Anchors | 
            DrivenTransformProperties.Pivot);

        // Setup the properties of the rect transform
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.pivot = Vector2.one * 0.5f;
        rectTransform.sizeDelta = offset;
    }
    public void FadeIn()
    {

    }
    #endregion

    #region Monobehaviour Messages
    private void OnValidate()
    {
        UpdateUI();
    }
    #endregion
}
