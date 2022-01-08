using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MatrixFocusPreviewEffect : MonoBehaviour
{
    #region Private Properties
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
    private Color defaultColor;
    #endregion

    #region Monobehaviour Messages
    // This needs to execute right after instantiation
    // because the owning object disables it immediately
    private void Awake()
    {
        defaultColor = image.color;
        // Make sure the effect is right on the parent
        rectTransform.localPosition = Vector3.zero;
        // Change the sort so that this object is above other objects
        canvas.overrideSorting = true;
        canvas.sortingOrder = 10;
    }
    #endregion

    #region Public Methods
    public void FadeIn()
    {
        // Enable itself if not already enabled
        if (!gameObject.activeSelf) gameObject.SetActive(true);

        // Start the fade in routine
        fadeInRoutine = StartCoroutine(FadeInRoutine());
    }
    public void FadeOut()
    {
        if (fadeInRoutine != null)
        {
            StopCoroutine(fadeInRoutine);
            fadeInRoutine = null;
        }

        // Complete any active tweens
        rectTransform.DOComplete();
        image.DOComplete();

        // Set the initial size of the rect transform and color of the image
        rectTransform.localScale = Vector3.one * smallSize;
        image.color = defaultColor;

        // Change the scale over time
        rectTransform.DOScale(largeSize, sizeChangeTime);
        image.DOColor(TransparentColor, colorChangeTime).WaitForCompletion();
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
        yield return image.DOColor(defaultColor, sizeChangeTime).WaitForCompletion();

        // Make the image flash
        for(int i = 0; i < numFlashes; i++)
        {
            yield return image.DOColor(TransparentColor, colorChangeTime).WaitForCompletion();
            yield return image.DOColor(defaultColor, colorChangeTime).WaitForCompletion();
        }
    }
    #endregion
}
