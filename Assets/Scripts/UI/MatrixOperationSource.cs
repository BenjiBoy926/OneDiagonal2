using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class MatrixOperationSource : MatrixUIChild, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    #region Public Properties
    public MatrixOperation Operation => operationGetter.Invoke();
    public bool IsCurrentOperationSource => MatrixParent.IsCurrentOperationSource(this);
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Rect transform to punch the scale of when the operation begins, and destination is set")]
    private List<RectTransform> rectTransforms;
    [SerializeField]
    [Tooltip("List of graphics to change color when this is used as the operation source")]
    private List<Graphic> graphics;
    #endregion

    #region Private Fields
    private Func<MatrixOperation> operationGetter = () => MatrixOperation.RowSwap(-1, -1);
    // Default colors for each of the graphics in the list
    private List<Color> defaultColors = new List<Color>();
    #endregion

    #region Public Methods
    public void Setup(Func<MatrixOperation> operationGetter)
    {
        Start();
        this.operationGetter = operationGetter;

        // Punch size each time that the destination is set, if this is the operation source
        MatrixParent.OnOperationDestinationSet.AddListener(() =>
        {
            if (IsCurrentOperationSource) PunchSize();
        });

        // Add the color of each graphic to the list of default colors
        foreach(Graphic graphic in graphics)
        {
            defaultColors.Add(graphic.color);
        }
    }

    // When the source begins dragging, set the intended next operation
    public void OnBeginDrag(PointerEventData data)
    {
        MatrixParent.SetOperationSource(this);

        // Set the color of the graphics this source is responsible for
        foreach(Graphic graphic in graphics)
        {
            graphic.color = MatrixParent.OperationColor(Operation.type);
        }
        PunchSize();
    }
    // We only have this so that BeginDrag and EndDrag actually work
    public void OnDrag(PointerEventData data)
    {
        // Do literally nothing at all
    }
    // When the source stops dragging, ask the matrix to confirm operation
    public void OnEndDrag(PointerEventData data)
    {
        bool success = MatrixParent.ConfirmOperation();
        if (success) PunchSize();

        // Reset graphics back to defaults
        for(int i = 0; i < graphics.Count; i++)
        {
            graphics[i].color = defaultColors[i];
        }
    }
    #endregion

    #region Private Methods
    private void PunchSize()
    {
        foreach(RectTransform rt in rectTransforms)
        {
            rt.DOComplete();
            rt.DOPunchScale(Vector3.one * 0.1f, MatrixParent.ScalePunchTime);
        }
    }
    #endregion
}
