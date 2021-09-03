using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MatrixDiagonalHighlightEffect : MatrixUIChild
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Reference to the canvas that puts the effect in front of all ui elements")]
    private Canvas canvas;
    [SerializeField]
    [Tooltip("Reference to the rect transform to change scale on")]
    private RectTransform rectTransform;
    [SerializeField]
    [Tooltip("Image to change the color for")]
    private Image image;
    [SerializeField]
    [Tooltip("Script that helps draw a line between the desired points")]
    private UILine line;

    [Space]

    [SerializeField]
    [Tooltip("Time it takes for the effect to shrink to its target size")]
    private float shrinkTime;
    [SerializeField]
    [Tooltip("Starting scale of the effect")]
    private float startingScale = 3f;
    [SerializeField]
    [Tooltip("Ending scale of the effect")]
    private float endingScale = 1f;
    #endregion

    #region Monobehaviour Messages
    protected override void Start()
    {
        base.Start();
        // Override sorting so the object is in front
        canvas.overrideSorting = true;
        canvas.sortingOrder = 10;
        
        // Get the start position
        Vector3 start = MatrixParent.RowUIs[0].ItemUIs[0].transform.position;
        // Get the end position
        MatrixRowUI lastRow = MatrixParent.RowUIs[MatrixParent.RowUIs.Length - 1];
        Vector3 end = lastRow.ItemUIs[lastRow.ItemUIs.Length - 1].transform.position;
        // Set the positions of the line
        line.SetPoints(start, end);

        // Set the starting scale of the rect transform
        rectTransform.localScale = Vector3.one * startingScale;

        // Set the starting color of the image
        Color transparentColor = new Color(image.color.r, image.color.g, image.color.b, 0f);
        Color targetColor = image.color;
        image.color = transparentColor;

        // Do scale and color
        rectTransform.DOScale(endingScale, shrinkTime);
        image.DOColor(targetColor, shrinkTime);
    }
    #endregion
}
