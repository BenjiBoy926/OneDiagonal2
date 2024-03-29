using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using TMPro;

public class MatrixOperationUI : MatrixUIChild
{
    #region Private Properties
    private string DisplayFormat => MatrixParent.IntendedNextOperationType switch
    {
        MatrixOperation.Type.Swap => "{0} {2} {1}",
        MatrixOperation.Type.Scale => "{0} = {1} {2} {0}",
        MatrixOperation.Type.Add => "{1} = {1} {2} {0}",
        _ => ""
    };
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Text used to display the operation in progress")]
    private TextMeshProUGUI text;
    #endregion

    #region Public Methods
    public void ShowText(MatrixOperation operation, string format = "{0}")
    {
        string sprite;
        string sourceRowName = RowName(operation.sourceRow);
        string destinationRowName = RowName(operation.destinationRow);
        format = string.Format(format, DisplayFormat);

        switch (operation.type)
        {
            case MatrixOperation.Type.Swap:
                sprite = "<sprite=\"swap icon\" index=0>";
                text.text = string.Format(format, sourceRowName, destinationRowName, sprite);
                break;
            case MatrixOperation.Type.Scale:
                sprite = "<sprite=\"scale icon\" index=0>";
                text.text = string.Format(format, destinationRowName, operation.scalar, sprite);
                break;
            case MatrixOperation.Type.Add:
                if (operation.scalar < Fraction.zero)
                {
                    sprite = "<sprite=\"subtract icon\" index=0>";
                }
                else sprite = "<sprite=\"add icon\" index=0>";

                text.text = string.Format(format, sourceRowName, destinationRowName, sprite);
                break;
        }

        text.color = UISettings.GetOperatorColor(operation.type);
    }
    public void HideText() => text.text = "";
    #endregion

    #region Child Overrides
    protected override void Start()
    {
        base.Start();

        HideText();

        // Listen for operation events
        MatrixParent.OnOperationStart.AddListener(OnOperationStart);
        MatrixParent.OnOperationDestinationSet.AddListener(OnOperationDestinationSet);
        MatrixParent.OnOperationDestinationUnset.AddListener(OnOperationDestinationUnset);
        MatrixParent.OnOperationFinish.AddListener(OnOperationFinished);
    }
    #endregion

    #region Event Listeners
    private void OnOperationStart()
    {
        ShowCurrentMatrixOperation();
    }
    private void OnOperationDestinationSet()
    {
        ShowCurrentMatrixOperation();
        UISettings.PunchOperator(text.transform);
    }
    // Operation unset is the same as operation confirm -
    // just disable the lines
    private void OnOperationDestinationUnset()
    {
        ShowCurrentMatrixOperation();
    }
    private void OnOperationFinished(bool success)
    {
        // Disable all of the lines
        HideText();
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
    private void ShowCurrentMatrixOperation()
    {
        ShowText(MatrixParent.IntendedNextOperation);
    }
    #endregion
}
