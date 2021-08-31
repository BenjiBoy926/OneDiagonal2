using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MatrixFocusEffect : MonoBehaviour
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Reference to the canvas used to put this effect over everything")]
    private Canvas canvas;
    [SerializeField]
    [Tooltip("Rect transform to grow/shrink for the focus effect")]
    private RectTransform rectTransform;
    [SerializeField]
    [Tooltip("Image to change color on for the focus effect")]
    private Image image;

    [Space]

    [SerializeField]
    [Tooltip("Starting scale of the focus effect")]
    private float startingScale = 5f;
    [SerializeField]
    [Tooltip("Ending scale of the focus after it finishes shrinking")]
    private float shrinkEndScale = 1f;
    [SerializeField]
    [Tooltip("Ending scale of the focus after if finishes growing")]
    private float growEndScale = 3f;

    [Space]

    [SerializeField]
    [Tooltip("Time it takes for the focus effect to shrink from big size to little size")]
    private float shrinkTime = 0.3f;
    [SerializeField]
    [Tooltip("Time it takes for the focus effect to grow and disappear")]
    private float growTime = 1f;
    #endregion

    #region Private Methods
    private void Start()
    {
        // Override canvas sorting so that the effect is over everything
        canvas.overrideSorting = true;
        canvas.sortingOrder = 100;
        StartCoroutine(FocusRoutine());
    }
    private IEnumerator FocusRoutine()
    {
        // Set transparent and ending color versions
        Color transparent = new Color(image.color.r, image.color.g, image.color.b, 0f);
        Color endingColor = new Color(image.color.r, image.color.g, image.color.b, 0.5f);

        // Start the correct scale and color
        rectTransform.localScale = Vector3.one * startingScale;
        image.color = transparent;

        // Tween the scale and color
        rectTransform.DOScale(shrinkEndScale, shrinkTime);
        yield return image.DOColor(endingColor, shrinkTime).WaitForCompletion();

        // Tween the scale and color again
        rectTransform.DOScale(growEndScale, growTime);
        yield return image.DOColor(transparent, growTime).WaitForCompletion();

        // Remove the game object
        Destroy(gameObject);
    }
    #endregion
}
