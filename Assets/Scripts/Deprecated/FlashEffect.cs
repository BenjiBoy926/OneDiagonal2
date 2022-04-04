using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FlashEffect : MonoBehaviour
{
    #region Public Methods
    public Image Image => image;
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Reference to the rect transform on this flash effect")]
    private RectTransform rectTransform;
    [SerializeField]
    [Tooltip("Image for the flash effect display")]
    private Image image;

    [Space]

    [SerializeField]
    [Tooltip("Time it takes for the flash effect to complete")]
    private float flashTime = 0.2f;
    [SerializeField]
    [Tooltip("Final scale of the flash before it is destroyed")]
    private float finalScale = 5f;
    #endregion

    #region Monobehaviour Messages
    private void Start()
    {
        UpdateUI();
        image.color = Color.clear;
    }
    private void OnValidate()
    {
        UpdateUI();
    }
    #endregion

    #region Public Methods
    public void UpdateUI()
    {
        // Stretch the rect transform across the whole parent
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.sizeDelta = Vector2.zero;
        // Pivot around the center
        rectTransform.pivot = Vector2.one * 0.5f;
        rectTransform.anchoredPosition = Vector2.zero;
        // Ensure correct starting scale
        rectTransform.localScale = Vector3.one;
    }
    public void Flash(Color color)
    {
        // Kill any animations that are still active
        rectTransform.DOKill();
        image.DOKill();

        // End color of the flash
        image.color = color;
        Color endColor = color.SetAlpha(0f);
        rectTransform.localScale = Vector3.one;

        // Scale the rect transform and change the color over time
        rectTransform.DOScale(finalScale, flashTime);
        image.DOColor(endColor, flashTime).WaitForCompletion();
    }
    #endregion
}
