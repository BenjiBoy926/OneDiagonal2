using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class MatrixRowUI : MatrixUIChild, IPointerEnterHandler, IPointerExitHandler
{
    #region Public Properties
    public Fraction[] CurrentRow => MatrixParent.CurrentMatrix.GetRow(rowIndex);
    public Fraction[] PreviewRow => MatrixParent.PreviewMatrix.GetRow(rowIndex);
    public int RowIndex => rowIndex;
    public bool IsCurrentOperationDestination => MatrixParent.IsCurrentOperationDestination(this);
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Prefab instantiated for each matrix item")]
    private MatrixItemUI itemUIPrefab;
    [SerializeField]
    [Tooltip("Layout group that the items are instantiated into")]
    private RectTransform rowRectTransform;
    [SerializeField]
    [Tooltip("Reference to the graphic to change color on when the row is set as the destination")]
    private Graphic rowGraphic;
    [SerializeField]
    [Tooltip("Script used to make the row a source of matrix operations")]
    private MatrixOperationSource rowOperationSource;
    [SerializeField]
    [Tooltip("Script used for adding this row to another row")]
    private MatrixRowAddWidget rowAddWidget;
    #endregion

    #region Private Fields
    private int rowIndex;
    private MatrixItemUI[] itemUIs;
    private Color defaultGraphicColor;
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
        defaultGraphicColor = rowGraphic.color;
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
            // Set the color
            rowGraphic.color = MatrixParent.OperationColor(MatrixParent.IntendedNextOperationType);
            PunchSize();
        }
    }
    public void OnPointerExit(PointerEventData data)
    {
        MatrixParent.UnsetOperationDestination();

        // Set the color back to normal if this is not the operation source
        if(!rowOperationSource.IsCurrentOperationSource && !rowAddWidget.IsCurrentOperationSource)
        {
            rowGraphic.color = defaultGraphicColor;
        }
    }
    #endregion

    #region Private Methods
    private void OnMatrixOperationFinished(bool success)
    {
        // Gotta set the color back to normal
        rowGraphic.color = defaultGraphicColor;

        // If operation finish succeeds with this as the destination, then punch the size
        if (success && IsCurrentOperationDestination) PunchSize();
    }
    private void PunchSize()
    {
        rowRectTransform.DOComplete();
        rowRectTransform.DOPunchScale(Vector3.one * MatrixParent.ScalePunchStrength, MatrixParent.ScalePunchTime);
    }
    #endregion
}
