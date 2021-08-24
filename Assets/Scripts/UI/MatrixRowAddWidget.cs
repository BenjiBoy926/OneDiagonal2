using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatrixRowAddWidget : MatrixUIChild
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Reference to the script that sets this widget as an operation source")]
    private MatrixOperationSource operationSource;
    #endregion

    public void Setup(int rowIndex)
    {
        base.Start();

        // Setup the operation source to request a row add operation
        operationSource.Setup(() => MatrixOperation.RowAdd(rowIndex, -1, Fraction.one));
    }
}
