using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MatrixRowUI : MatrixUIChild, IPointerEnterHandler, IPointerExitHandler
{
    #region Public Properties
    public Fraction[] CurrentRow => MatrixParent.CurrentMatrix.GetRow(row);
    public Fraction[] PreviewRow => MatrixParent.PreviewMatrix.GetRow(row);
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Prefab instantiated for each matrix item")]
    private MatrixItemUI itemUIPrefab;
    [SerializeField]
    [Tooltip("Layout group that the items are instantiated into")]
    private LayoutGroup itemParent;
    [SerializeField]
    [Tooltip("Script used to make the row a source of matrix operations")]
    private MatrixOperationSource operationSource;
    #endregion

    #region Private Fields
    private int row;

    private MatrixItemUI[] itemUIs;
    #endregion

    #region Public Methods
    public void Setup( int row)
    {
        Start();

        this.row = row;

        // Initialize the lsit of item uis
        itemUIs = new MatrixItemUI[MatrixParent.CurrentMatrix.cols];

        for(int j = 0; j < MatrixParent.CurrentMatrix.cols; j++)
        {
            MatrixItemUI itemUI = Instantiate(itemUIPrefab, itemParent.transform);
            itemUI.Setup(this, j);
            itemUIs[j] = itemUI;
        }

        // Setup the operation source to request a row swap when dragged
        operationSource.Setup(() => MatrixOperation.RowSwap(row, -1));
    }

    public void OnPointerEnter(PointerEventData data)
    {
        // Do not do this if this row is the source
        MatrixParent.SetOperationDestination(this);
    }
    public void OnPointerExit(PointerEventData data)
    {
        // Check if this is the source first, and if it is then do this
        MatrixParent.UnsetOperationDestination();
    }
    #endregion
}
