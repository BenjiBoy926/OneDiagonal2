using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MatrixItemUI : MatrixUIChild
{
    #region Public Properties
    public Fraction CurrentFraction => rowParent.CurrentRow[col];
    public Fraction PreviewFraction => rowParent.PreviewRow[col];
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Text used to display the current matrix item")]
    private TextMeshProUGUI text;
    #endregion

    #region Private Fields
    private MatrixRowUI rowParent;
    private int col;
    #endregion

    public void Setup(MatrixRowUI parent, int col)
    {
        Start();

        this.rowParent = parent;
        this.col = col;

        text.text = CurrentFraction.ToString();
    }
}
