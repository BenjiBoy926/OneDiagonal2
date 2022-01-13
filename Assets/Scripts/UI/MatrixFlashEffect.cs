using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MatrixFlashEffect : MonoBehaviour
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Reference to the rect transform on this flash effect")]
    private RectTransform rectTransform;
    [SerializeField]
    [Tooltip("Canvas used to place this object on top of other objects")]
    private Canvas canvas;
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

    #region Public Methods
    public void Flash(Color color)
    {
        image.color = color;

        // Start the flash routine
        StartCoroutine(FlashRoutine());
    }
    #endregion

    #region Private Methods
    private void Start()
    {
        // Sort the flash to be above other things
        canvas.overrideSorting = true;
        canvas.sortingOrder = 10;

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
    private IEnumerator FlashRoutine()
    {
        // End color of the flash
        Color endColor = new Color(image.color.r, image.color.g, image.color.b, 0f);

        // Scale the rect transform and change the color over time
        rectTransform.DOScale(finalScale, flashTime);
        yield return image.DOColor(endColor, flashTime).WaitForCompletion();

        // Destroy this game object
        Destroy(gameObject);
    }
    #endregion
}
