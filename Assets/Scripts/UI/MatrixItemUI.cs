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
    [Tooltip("Text used to display the current matrix item")]
    private TextMeshProUGUI text;
    #endregion

    #region Private Fields
    private MatrixRowUI rowParent;
    private int columnIndex;
    #endregion

    #region Public Methods
    public void Setup(MatrixRowUI rowParent, int columnIndex)
    {
        Start();

        this.rowParent = rowParent;
        this.columnIndex = columnIndex;

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
        text.text = PreviewFraction.ToString();
        SetColor(PreviewFraction);
    }
    #endregion

    #region Private Methods
    private void SetColor(Fraction displayedFraction)
    {
        // Check if we are on the diagonal
        if(columnIndex == rowParent.RowIndex)
        {
            // If we are on the diagonal and displaying a one, then set the good color
            if (displayedFraction == Fraction.one) text.color = GameSettings.DiagonalColors.goodColor;
            // If we are on the diagonal but not displaying a one then set the bad color
            else text.color = GameSettings.DiagonalColors.badColor;
        }
        else
        {
            // If we are not on the diagonal and displaying a zero then set the good color
            if (displayedFraction == Fraction.zero) text.color = GameSettings.NotDiagonalColors.goodColor;
            // If we are not on the diagonal and not displaying a zero then set the bad color
            else text.color = GameSettings.NotDiagonalColors.badColor;
        }
    }
    #endregion
}
