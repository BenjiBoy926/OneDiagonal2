using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;

public class MatrixRowUI : MatrixUIChild
{
    #region Public Properties
    public MatrixItemUI[] ItemUIs => itemUIs;
    public Fraction[] CurrentRow => MatrixParent.CurrentMatrix.GetRow(rowIndex);
    public Fraction[] PreviewRow => MatrixParent.PreviewMatrix.GetRow(rowIndex);
    public int RowIndex => rowIndex;
    public bool IsCurrentOperationDestination => MatrixParent.OperationInProgress && MatrixParent.OperationDestination == this;
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Select for this matrix row that determines if it will react on mouse events")]
    private Selectable selectable;
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
    [Tooltip("Script used to make the row a source of matrix operations")]
    private MatrixOperationSource rowOperationSource;
    [SerializeField]
    [Tooltip("Script used for adding this row to another row")]
    private MatrixRowAddWidget rowAddWidget;
    [SerializeField]
    [Tooltip("Event trigger used to detect mouse enter/exit")]
    private EventTrigger mouseDetector;
    [SerializeField]
    [Tooltip("Root object for all row elements that are not in the layout")]
    private Transform nonLayoutObjects;
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

        // Listen for start and end of an operation
        MatrixParent.OnOperationStart.AddListener(OnMatrixOperationStart);
        MatrixParent.OnOperationFinish.AddListener(OnMatrixOperationFinish);

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

        // Setup the entry from on pointer enter
        EventTrigger.Entry pointerEnter = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerEnter,
            callback = new EventTrigger.TriggerEvent()
        };
        pointerEnter.callback.AddListener(data => OnPointerEnter());
        mouseDetector.triggers.Add(pointerEnter);

        // Setup the entry from on pointer exit
        EventTrigger.Entry pointerExit = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerExit,
            callback = new EventTrigger.TriggerEvent()
        };
        pointerExit.callback.AddListener(data => OnPointerExit());
        mouseDetector.triggers.Add(pointerExit);

        // Set non layout objects to last sibling
        nonLayoutObjects.SetAsLastSibling();
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
    #endregion

    #region Event Listeners
    private void OnPointerEnter()
    {
        MatrixParent.SetOperationDestination(this);
    }
    private void OnPointerExit()
    {
        MatrixParent.UnsetOperationDestination();
    }
    private void OnMatrixOperationStart()
    {
        // Disable this row if we are adding this row to another row
        if (MatrixParent.IntendedNextOperationType == MatrixOperation.Type.Add && 
            MatrixParent.IntendedNextOperation.sourceRow == rowIndex) 
            selectable.interactable = false;
    }
    private void OnMatrixOperationFinish(bool success)
    {
        selectable.interactable = true;
    }
    #endregion
}
