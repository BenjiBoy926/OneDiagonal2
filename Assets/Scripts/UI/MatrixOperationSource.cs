using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MatrixOperationSource : MatrixUIChild, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    #region Public Properties
    public MatrixOperation GetOperation => operationGetter.Invoke();
    // Add a property that checks if this source is the source currently on the matrix ui
    #endregion

    #region Private Fields
    private Func<MatrixOperation> operationGetter = () => MatrixOperation.RowSwap(-1, -1);
    #endregion

    public void Setup(Func<MatrixOperation> operationGetter)
    {
        Start();
        this.operationGetter = operationGetter;
    }

    // When the source begins dragging, set the intended next operation
    public void OnBeginDrag(PointerEventData data)
    {
        MatrixParent.SetOperationSource(this);
    }
    // We only have this so that BeginDrag and EndDrag actually work
    public void OnDrag(PointerEventData data)
    {
        // Do literally nothing at all
    }
    // When the source stops dragging, ask the matrix to confirm operation
    public void OnEndDrag(PointerEventData data)
    {
        MatrixParent.ConfirmOperation();
    }
}
