using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;

public class MatrixRowUI : MatrixUIChild, IPointerEnterHandler, IPointerExitHandler
{
    #region Public Properties
    public MatrixItemUI[] ItemUIs => itemUIs;
    public Fraction[] CurrentRow => MatrixParent.CurrentMatrix.GetRow(rowIndex);
    public Fraction[] PreviewRow => MatrixParent.PreviewMatrix.GetRow(rowIndex);
    public int RowIndex => rowIndex;
    public bool IsCurrentOperationDestination => MatrixParent.IsCurrentOperationDestination(this);
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Text used to label this row")]
    private TextMeshProUGUI label;
    [SerializeField]
    [Tooltip("Prefab instantiated for each matrix item")]
    private MatrixItemUI itemUIPrefab;
    [SerializeField]
    [Tooltip("Layout group that the items are instantiated into")]
    private RectTransform rowRectTransform;
    [SerializeField]
    [Tooltip("Reference to the graphic to change color on when the row is set as the destination")]
    private Graphic outlineGraphic;
    [SerializeField]
    [Tooltip("Script used to make the row a source of matrix operations")]
    private MatrixOperationSource rowOperationSource;
    [SerializeField]
    [Tooltip("Script used for adding this row to another row")]
    private MatrixRowAddWidget rowAddWidget;

    [Header("Cursors")]

    [SerializeField]
    [Tooltip("Cursor texture used when moving a row down")]
    private CursorTexture cursorDown;
    [SerializeField]
    [Tooltip("Cursor texture used when moving a row up")]
    private CursorTexture cursorUp;
    [SerializeField]
    [Tooltip("Cursor texture used when multiplying a row")]
    private CursorTexture cursorMultiply;
    [SerializeField]
    [Tooltip("Cursor texture used when dividing a row")]
    private CursorTexture cursorDivide;
    [SerializeField]
    [Tooltip("Cursot texture used when adding a row to a row")]
    private CursorTexture cursorAdd;
    [SerializeField]
    [Tooltip("Cursor texture used when subtracting a row from a row")]
    private CursorTexture cursorSubstract;
    #endregion

    #region Private Fields
    private int rowIndex;
    private MatrixItemUI[] itemUIs;
    #endregion

    #region Public Methods
    public void Setup(int rowIndex)
    {
        Start();

        this.rowIndex = rowIndex;
        label.text = (rowIndex + 1).ToString();

        // Initialize the lsit of item uis
        itemUIs = new MatrixItemUI[MatrixParent.CurrentMatrix.cols];

        for(int j = 0; j < MatrixParent.CurrentMatrix.cols; j++)
        {
            MatrixItemUI itemUI = Instantiate(itemUIPrefab, rowRectTransform);
            itemUI.Setup(this, j);
            itemUIs[j] = itemUI;
        }

        // Setup the operation source to request a row swap when dragged
        rowOperationSource.Setup(() => MatrixOperation.RowSwap(-1, rowIndex));
        rowAddWidget.Setup(rowIndex);

        // Listen for operation end
        MatrixParent.OnOperationFinish.AddListener(OnMatrixOperationFinished);

        // Set the default graphic color
        outlineGraphic.enabled = false;
    }
    public void ShowCurrent()
    {
        foreach(MatrixItemUI item in itemUIs)
        {
            item.ShowCurrent();
        }
    }
    public void ShowPreview()
    {
        foreach(MatrixItemUI item in itemUIs)
        {
            item.ShowPreview();
        }
    }

    public void OnPointerEnter(PointerEventData data)
    {
        if(MatrixParent.SetOperationDestination(this))
        {
            // Enable the outline
            outlineGraphic.enabled = true;
            outlineGraphic.color = UISettings.GetOperatorColor(MatrixParent.IntendedNextOperationType);
            // Do a short grow animation
            UISettings.PunchOperator(transform);

            MatrixOperation intendedOperation = MatrixParent.IntendedNextOperation;

            // Change the cursor based on the type
            switch(intendedOperation.type)
            {
                case MatrixOperation.Type.Swap:
                    // If destination is above source then set cursor up 
                    if (intendedOperation.sourceRow > intendedOperation.destinationRow)
                    {
                        cursorUp.SetCursor();
                    }
                    // Otherwise set cursor down
                    else cursorDown.SetCursor();
                    break;
                case MatrixOperation.Type.Scale:
                    // If the scalar is between -1 and 1, then it counts as a division, so we use the divide cursor
                    if (intendedOperation.scalar > -Fraction.one && intendedOperation.scalar < Fraction.one)
                    {
                        cursorDivide.SetCursor();
                    }
                    else cursorMultiply.SetCursor();
                    break;
                case MatrixOperation.Type.Add:
                    // If scalar is bigger than zero than set add cursor
                    if (intendedOperation.scalar > Fraction.zero) cursorAdd.SetCursor();
                    // If scalar is smaller than zero then set subtract cursor
                    else cursorSubstract.SetCursor();
                    break;
            }
        }
    }
    public void OnPointerExit(PointerEventData data)
    {
        MatrixParent.UnsetOperationDestination();
        // In any case set cursor to default
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

        // Set the color back to normal if this is not the operation source
        if(!rowOperationSource.IsCurrentOperationSource && !rowAddWidget.IsCurrentOperationSource)
        {
            outlineGraphic.enabled = false;
        }
    }
    #endregion

    #region Private Methods
    private void OnMatrixOperationFinished(bool success)
    {
        // Gotta set the color back to normal
        outlineGraphic.enabled = false;

        // If operation finish succeeds with this as the destination, then punch the size
        if (success && IsCurrentOperationDestination)
        {
            UISettings.PunchOperator(transform);
            Instantiate(UISettings.FlashEffect, rowRectTransform);
        }
    }
    #endregion
}
