using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MatrixRowAddWidget : MatrixUIChild
{
    #region Private Properties
    private Fraction Scalar => adding ? Fraction.one : -Fraction.one;
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Reference to the script that sets this widget as an operation source")]
    private MatrixOperationSource operationSource;
    [SerializeField]
    [Tooltip("Text that displays a plus when adding and a minus when subtracting")]
    private TextMeshProUGUI text;
    #endregion

    #region Private Fields
    // True if adding and false if subtracting
    private bool adding = true;
    #endregion

    #region Public Methods
    public void Setup(int rowIndex)
    {
        base.Start();

        // Setup the operation source to request a row add operation
        operationSource.Setup(() => MatrixOperation.RowAdd(rowIndex, -1, Scalar));
        
        // Update display
        UpdateDisplay();
    }
    public void ToggleAdding()
    {
        adding = !adding;
        UpdateDisplay();
    }
    #endregion

    #region Private Methods
    private void UpdateDisplay()
    {
        if (adding) text.text = "+";
        else text.text = "-";
    }
    #endregion
}
