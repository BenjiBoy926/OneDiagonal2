using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MatrixFocusPreviewEffect : MonoBehaviour
{
    #region Private Properties
    private Color TranslucentColor => new Color(image.color.r, image.color.g, image.color.b, 0.5f);
    private Color TransparentColor => new Color(image.color.r, image.color.g, image.color.b, 0f);
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Reference to the rect transform to change the size for")]
    private RectTransform rectTransform;
    [SerializeField]
    [Tooltip("Reference to the canvas used to put this object on top of other objects")]
    private Canvas canvas;
    [SerializeField]
    [Tooltip("Reference to the image to change color for")]
    private Image image;

    [Space]

    [SerializeField]
    [Tooltip("Size of the effect when large")]
    private float largeSize = 2f;
    [SerializeField]
    [Tooltip("Size of the effect when small")]
    private float smallSize = 1f;

    [Space]

    [SerializeField]
    [Tooltip("Time it takes for the size to change")]
    private float sizeChangeTime = 0.2f;
    [SerializeField]
    [Tooltip("Time it takes for the image color to lerp alpha on each flash")]
    private float colorChangeTime = 0.5f;
    [SerializeField]
    [Tooltip("The number of flashes that the focus does before stabilizing to one color")]
    private int numFlashes = 5;
    #endregion

    #region Private Fields
    private Coroutine fadeInRoutine;
    #endregion

    #region Monobehaviour Messages
    private void Start()
    {
        // Make sure the effect is right on the parent
        rectTransform.localPosition = Vector3.zero;
        // Change the sort so that this object is above other objects
        canvas.overrideSorting = true;
        canvas.sortingOrder = 10;
        FadeIn();
    }
    #endregion

    #region Public Methods
    public void FadeIn()
    {
        fadeInRoutine = StartCoroutine(FadeInRoutine());
    }
    public void FadeOut()
    {
        if (fadeInRoutine != null) StopCoroutine(fadeInRoutine);
        StartCoroutine(FadeOutRoutine());
    }
    #endregion

    #region Private Methods
    private IEnumerator FadeInRoutine()
    {
        // Set initial scale and color
        rectTransform.localScale = Vector3.one * largeSize;
        image.color = TransparentColor;

        // Change to small size and wait for completion
        rectTransform.DOScale(smallSize, sizeChangeTime);
        yield return image.DOColor(TranslucentColor, sizeChangeTime).WaitForCompletion();

        // Make the image flash
        for(int i = 0; i < numFlashes; i++)
        {
            yield return image.DOColor(TransparentColor, colorChangeTime).WaitForCompletion();
            yield return image.DOColor(TranslucentColor, colorChangeTime).WaitForCompletion();
        }
    }
    private IEnumerator FadeOutRoutine()
    {
        // Complete any active tweens
        rectTransform.DOComplete();
        image.DOComplete();

        // Set the initial size of the rect transform and color of the image
        rectTransform.localScale = Vector3.one * smallSize;
        image.color = TranslucentColor;

        // Change the scale over time
        rectTransform.DOScale(largeSize, sizeChangeTime);
        yield return image.DOColor(TransparentColor, colorChangeTime).WaitForCompletion();

        // Destroy the game object
        Destroy(gameObject);
    }
    #endregion
}
