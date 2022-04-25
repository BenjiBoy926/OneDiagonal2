using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MatrixItemUI : MatrixUIChild
{
    #region Public Properties
    public Fraction CurrentFraction => rowParent.CurrentRow[columnIndex];
    public Fraction PreviewFraction => rowParent.PreviewRow[columnIndex];
    public int ColumnIndex => columnIndex;
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Reference to the object to create a focus effect on the matrix item when the matrix is solved")]
    private MatrixFocusEffect focusEffect;
    [SerializeField]
    [Tooltip("Smaller text that appears during a preview")]
    private TextMeshProUGUI smallText;
    [SerializeField]
    [Tooltip("Text used to display the current matrix item")]
    private TextMeshProUGUI mainText;
    [SerializeField]
    [Tooltip("Arrow that appears to suggest a transition during a preview")]
    private UILine previewArrow;
    [SerializeField]
    [Tooltip("Margin given to the arrow off to the left of the item graphic")]
    private float arrowMargin = 5;
    [SerializeField]
    [Tooltip("Width of the arrow")]
    private float arrowWidth = 10;
    [SerializeField]
    [Tooltip("Reference to the object used to create the preview focus effect")]
    private MatrixFocusPreviewEffect focusPreviewEffect;
    #endregion

    #region Private Fields
    private MatrixRowUI rowParent;
    private int columnIndex;
    private MatrixFocusPreviewEffect currentPreviewEffect;
    private Graphic[] arrowGraphics;
    #endregion

    #region Public Methods
    public void Setup(MatrixRowUI rowParent, int columnIndex)
    {
        Start();

        this.rowParent = rowParent;
        this.columnIndex = columnIndex;

        // Instantiate a preview effect to use when previewing the win state
        if (rowParent.RowIndex == columnIndex)
        {
            currentPreviewEffect = Instantiate(focusPreviewEffect, transform);
            currentPreviewEffect.gameObject.SetActive(false);
        }

        // Add listener for matrix events
        MatrixParent.OnMatrixSolved.AddListener(OnMatrixSolved);
        MatrixParent.OnOperationDestinationUnset.AddListener(OnMatrixOperationDestinationUnset);

        // Get a list of all graphics in the arrow
        arrowGraphics = previewArrow.GetComponentsInChildren<Graphic>(true);

        // Show the current fraction
        ShowCurrent();
    }
    public void ShowCurrent()
    {
        // Display no text anymore in the small area
        smallText.text = "";
        previewArrow.gameObject.SetActive(false);
        SetFraction(mainText, CurrentFraction);
    }
    public void ShowPreview(MatrixOperation operation)
    {
        // Set the text for the main text to the preview
        SetFraction(mainText, PreviewFraction);

        // Use the small text to display what the fraction was before
        if (CurrentFraction != PreviewFraction)
        {
            SetFraction(smallText, CurrentFraction);
            previewArrow.gameObject.SetActive(true);

            // Set the color of the arrow to the intended operation color
            foreach (Graphic graphic in arrowGraphics)
                graphic.color = UISettings.GetOperatorColor(operation.type);
        }

        // If this is along the diagonal and the preview is the identity then create the preview effect
        if (columnIndex == rowParent.RowIndex && MatrixParent.PreviewMatrix.isIdentity)
        {
            currentPreviewEffect.FadeIn();
        }
    }
    #endregion

    #region Monobehaviour Messages
    private void Update()
    {
        // We have to do this every frame because
        // apparently the text bounds do not update immediately
        if (previewArrow.gameObject.activeInHierarchy)
        {
            // Set the points on the ui line based on the bounds of the two text objects
            Vector3 topRight = smallText.bounds.GetPointInside(0f, 0.5f, 0f) + smallText.transform.position + (arrowMargin * Vector3.left);
            Vector3 topLeft = topRight + Vector3.left * arrowWidth;
            Vector3 bottomRight = mainText.bounds.GetPointInside(0f, 0.5f, 0f) + mainText.transform.position + (arrowMargin * Vector3.left);
            Vector3 bottomLeft = new Vector3(topLeft.x, bottomRight.y, bottomRight.z);
            previewArrow.SetPoints(topRight, topLeft, bottomLeft, bottomRight);
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (smallText)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(smallText.bounds.center + smallText.transform.position, smallText.bounds.size);
        }

        if (mainText)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(mainText.bounds.center + mainText.transform.position, mainText.bounds.size);
        }
    }
    #endregion

    #region Private Methods
    private void SetFraction(TextMeshProUGUI textComponent, Fraction displayedFraction)
    {
        textComponent.text = displayedFraction.ToString();

        // Check if we are on the diagonal
        if (columnIndex == rowParent.RowIndex)
        {
            // If we are on the diagonal and displaying a one, then set the good color
            if (displayedFraction == Fraction.one) textComponent.color = UISettings.DiagonalColors.goodColor;
            // If we are on the diagonal but not displaying a one then set the bad color
            else textComponent.color = UISettings.DiagonalColors.badColor;
        }
        else
        {
            // If we are not on the diagonal and displaying a zero then set the good color
            if (displayedFraction == Fraction.zero) textComponent.color = UISettings.NotDiagonalColors.goodColor;
            // If we are not on the diagonal and not displaying a zero then set the bad color
            else textComponent.color = UISettings.NotDiagonalColors.badColor;
        }
    }
    private void OnMatrixSolved()
    {
        if(columnIndex == rowParent.RowIndex)
        {
            MatrixFocusEffect effect = Instantiate(focusEffect, transform);
            effect.transform.localPosition = Vector3.zero;

            // Fade out the current preview effect
            if (currentPreviewEffect) currentPreviewEffect.FadeOut();
        }
    }
    private void OnMatrixOperationDestinationUnset()
    {
        if (MatrixParent.PreviewMatrix.isIdentity && currentPreviewEffect) currentPreviewEffect.FadeOut();
    }
    #endregion
}
