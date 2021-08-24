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
    public void Setup(MatrixRowUI parent, int columnIndex)
    {
        Start();

        this.rowParent = parent;
        this.columnIndex = columnIndex;

        text.text = CurrentFraction.ToString();
    }
    public void ShowCurrent()
    {
        text.text = CurrentFraction.ToString();
    }
    public void ShowPreview()
    {
        text.text = PreviewFraction.ToString();
    }
    #endregion
}
