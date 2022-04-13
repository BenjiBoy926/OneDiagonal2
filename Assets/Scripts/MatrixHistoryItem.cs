using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MatrixHistoryItem
{
    #region Public Properties
    public Matrix Matrix => matrix;
    public MatrixOperation PreviousOperation => previousOperation;
    public bool IsInitialItem => !previousOperation.IsValid;
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Matrix at this step in the history")]
    private Matrix matrix;
    [SerializeField]
    [Tooltip("Operation that caused this matrix to occur")]
    private MatrixOperation previousOperation;
    #endregion

    #region Constructors
    public MatrixHistoryItem(Matrix matrix) : this(matrix, MatrixOperation.Invalid) { }
    public MatrixHistoryItem(Matrix matrix, MatrixOperation previousOperation)
    {
        this.matrix = matrix;
        this.previousOperation = previousOperation;
    }
    #endregion
}
