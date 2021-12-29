using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using TMPro;

public class MatrixOperationUI : MatrixUIChild
{
    #region Private Properties
    private string DisplayFormat => MatrixParent.IntendedNextOperationType switch
    {
        MatrixOperation.Type.Swap => "{0} <-> {1}",
        MatrixOperation.Type.Scale => "{0} = {1} * {0}",
        MatrixOperation.Type.Add => "{1} = {1} {2} {0}",
        _ => ""
    };
    private string SourceRowName => RowName(MatrixParent.IntendedNextOperation.sourceRow);
    private string DestinationRowName => RowName(MatrixParent.IntendedNextOperation.destinationRow);
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Text used to display the operation in progress")]
    private TextMeshProUGUI text;
    #endregion

    #region Child Overrides
    protected override void Start()
    {
        base.Start();

        text.text = "";

        // Listen for operation events
        MatrixParent.OnOperationStart.AddListener(OnOperationStart);
        MatrixParent.OnOperationDestinationSet.AddListener(OnOperationDestinationSet);
        MatrixParent.OnOperationDestinationUnset.AddListener(OnOperationDestinationUnset);
        MatrixParent.OnOperationFinish.AddListener(x => OnOperationFinished());
    }
    #endregion

    #region Event Listeners
    private void OnOperationStart()
    {
        UpdateText();
        text.color = UISettings.GetOperatorColor(MatrixParent.IntendedNextOperationType);
    }
    private void OnOperationDestinationSet()
    {
        UpdateText();
        UISettings.PunchOperator(text.transform);
    }
    // Operation unset is the same as operation confirm -
    // just disable the lines
    private void OnOperationDestinationUnset()
    {
        UpdateText();
    }
    private void OnOperationFinished()
    {
        // Disable all of the lines
        text.text = "";
    }
    #endregion

    #region Helper Methods
    private string RowName(int row)
    {
        if (row >= 0 && row < MatrixParent.Rows)
        {
            return $"R{row + 1}";
        }
        else return "?";
    }
    private void UpdateText()
    {
        MatrixOperation intendedOperation = MatrixParent.IntendedNextOperation;

        switch(intendedOperation.type)
        {
            case MatrixOperation.Type.Swap:
                text.text = string.Format(DisplayFormat, SourceRowName, DestinationRowName);
                break;
            case MatrixOperation.Type.Scale:
                text.text = string.Format(DisplayFormat, DestinationRowName, intendedOperation.scalar);
                break;
            case MatrixOperation.Type.Add:
                char symbol = intendedOperation.scalar < Fraction.zero ? '-' : '+';
                text.text = string.Format(DisplayFormat, SourceRowName, DestinationRowName, symbol);
                break;
        }
    }
    #endregion
}
