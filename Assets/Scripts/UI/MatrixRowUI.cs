using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MatrixRowUI : MatrixUIChild, IPointerEnterHandler, IPointerExitHandler
{
    #region Public Properties
    public Fraction[] CurrentRow => MatrixParent.CurrentMatrix.GetRow(rowIndex);
    public Fraction[] PreviewRow => MatrixParent.PreviewMatrix.GetRow(rowIndex);
    public int RowIndex => rowIndex;
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Prefab instantiated for each matrix item")]
    private MatrixItemUI itemUIPrefab;
    [SerializeField]
    [Tooltip("Layout group that the items are instantiated into")]
    private RectTransform itemParent;
    [SerializeField]
    [Tooltip("Script used to make the row a source of matrix operations")]
    private MatrixOperationSource operationSource;
    [SerializeField]
    [Tooltip("Script used for adding this row to another row")]
    private MatrixRowAddWidget rowAddWidget;
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

        // Initialize the lsit of item uis
        itemUIs = new MatrixItemUI[MatrixParent.CurrentMatrix.cols];

        for(int j = 0; j < MatrixParent.CurrentMatrix.cols; j++)
        {
            MatrixItemUI itemUI = Instantiate(itemUIPrefab, itemParent);
            itemUI.Setup(this, j);
            itemUIs[j] = itemUI;
        }

        // Setup the operation source to request a row swap when dragged
        operationSource.Setup(() => MatrixOperation.RowSwap(-1, rowIndex));
        rowAddWidget.Setup(rowIndex);
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
        MatrixParent.SetOperationDestination(this);
    }
    public void OnPointerExit(PointerEventData data)
    {
        MatrixParent.UnsetOperationDestination();
    }
    #endregion
}
