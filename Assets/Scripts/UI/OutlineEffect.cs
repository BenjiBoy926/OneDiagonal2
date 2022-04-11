using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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
    public void FadeIn(Color color)
    {
        // Kill any active tweens
        KillTweens();

        // Set the initial values
        image.color = color.SetAlpha(0f);
        transform.localScale = Vector3.one * fadeOutScale;

        // Animate towards normal scale and target color
        transform.DOScale(1f, fadeTime);
        image.DOColor(color, fadeTime);
    }
    public void FadeOut(Color color)
    {
        // Kill any active tweens
        KillTweens();

        // Set the initial values
        image.color = color.SetAlpha(1f);
        transform.localScale = Vector3.one;

        // Animate towards normal scale and target color
        transform.DOScale(fadeOutScale, fadeTime);
        image.DOColor(image.color.SetAlpha(0f), fadeTime);
    }
    #endregion

    #region Monobehaviour Messages
    private void KillTweens()
    {
        transform.DOKill();
        image.DOKill();
    }
    private void OnValidate()
    {
        UpdateUI();
    }
    private void OnDestroy()
    {
        KillTweens();
    }
    #endregion
}
