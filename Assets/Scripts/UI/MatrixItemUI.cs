using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    [Tooltip("Text used to display the current matrix item")]
    private TextMeshProUGUI text;
    [SerializeField]
    [Tooltip("Reference to the object used to create the preview focus effect")]
    private MatrixFocusPreviewEffect focusPreviewEffect;
    #endregion

    #region Private Fields
    private MatrixRowUI rowParent;
    private int columnIndex;
    private MatrixFocusPreviewEffect currentPreviewEffect;
    #endregion

    #region Public Methods
    public void Setup(MatrixRowUI rowParent, int columnIndex)
    {
        Start();

        this.rowParent = rowParent;
        this.columnIndex = columnIndex;

        // Add listener for matrix events
        MatrixParent.OnMatrixSolved.AddListener(OnMatrixSolved);
        MatrixParent.OnOperationDestinationUnset.AddListener(OnMatrixOperationDestinationUnset);

        // Show the current fraction
        ShowCurrent();
    }
    public void ShowCurrent()
    {
        text.text = CurrentFraction.ToString();
        SetColor(CurrentFraction);
    }
    public void ShowPreview()
    {
        // Get the intended next operation
        //MatrixOperation intendedOperation = MatrixParent.IntendedNextOperation;
        //// Get the fraction that will be operating on this fraction, if any
        //Fraction operatingFraction = new Fraction();
        //bool hasOperatingFraction = intendedOperation.OperatingFraction(MatrixParent.CurrentMatrix, 
        //    rowParent.RowIndex, columnIndex, ref operatingFraction);

        //// If this matrix item is being operated on, then display the full operation
        //if (hasOperatingFraction)
        //{
        //    text.text = $"{CurrentFraction} {intendedOperation.OperationString} {operatingFraction} = {PreviewFraction}";
        //}
        //// If this matrix item is not affected by the operation, display the fraction alone
        //else text.text = PreviewFraction.ToString();

        text.text = PreviewFraction.ToString();
        SetColor(PreviewFraction);

        // If this is along the diagonal and the preview is the identity then create the preview effect
        if(columnIndex == rowParent.RowIndex && MatrixParent.PreviewMatrix.isIdentity)
        {
            currentPreviewEffect = Instantiate(focusPreviewEffect, transform);
        }
    }
    #endregion

    #region Private Methods
    private void SetColor(Fraction displayedFraction)
    {
        // Check if we are on the diagonal
        if(columnIndex == rowParent.RowIndex)
        {
            // If we are on the diagonal and displaying a one, then set the good color
            if (displayedFraction == Fraction.one) text.color = UISettings.DiagonalColors.goodColor;
            // If we are on the diagonal but not displaying a one then set the bad color
            else text.color = UISettings.DiagonalColors.badColor;
        }
        else
        {
            // If we are not on the diagonal and displaying a zero then set the good color
            if (displayedFraction == Fraction.zero) text.color = UISettings.NotDiagonalColors.goodColor;
            // If we are not on the diagonal and not displaying a zero then set the bad color
            else text.color = UISettings.NotDiagonalColors.badColor;
        }
    }
    private void OnMatrixSolved()
    {
        if(columnIndex == rowParent.RowIndex)
        {
            MatrixFocusEffect effect = Instantiate(focusEffect, transform);
            effect.transform.localPosition = Vector3.zero;
        }
    }
    private void OnMatrixOperationDestinationUnset()
    {
        if (currentPreviewEffect) currentPreviewEffect.FadeOut();
    }
    #endregion
}
