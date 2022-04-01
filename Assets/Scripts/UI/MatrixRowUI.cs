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
    [Tooltip("Prefab used to create a flash effect on the matrix row when an operation is confirmed")]
    private ButtonEffects effects;
    [SerializeField]
    [Tooltip("Reference to the graphic to change color on when the row is set as the destination")]
    private Graphic outlineGraphic;
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

    public void OnPointerEnter()
    {
        if(MatrixParent.SetOperationDestination(this))
        {
            // Enable the outline
            outlineGraphic.enabled = true;
            outlineGraphic.color = UISettings.GetOperatorColor(MatrixParent.IntendedNextOperationType);
        }
    }
    public void OnPointerExit()
    {
        MatrixParent.UnsetOperationDestination();

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
            effects.Play(
                UISettings.GetOperatorColor(MatrixParent.IntendedNextOperationType),
                ButtonSound.Confirm);
        }
    }
    #endregion
}
