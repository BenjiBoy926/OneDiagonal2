using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class MatrixOperationSource : MatrixUIChild, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    #region Public Properties
    public MatrixOperation Operation => operationGetter.Invoke();
    public bool IsCurrentOperationSource => MatrixParent.OperationInProgress && MatrixParent.OperationSource == this;
    #endregion

    #region Private Fields
    private Func<MatrixOperation> operationGetter = () => MatrixOperation.RowSwap(-1, -1);
    #endregion

    #region Public Methods
    public void Setup(Func<MatrixOperation> operationGetter)
    {
        Start();
        this.operationGetter = operationGetter;
    }
    #endregion

    #region Pointer Interface Implementations
    public void OnPointerDown(PointerEventData data)
    {
        if (!MatrixParent.OperationInProgress)
        {
            MatrixParent.StartOperation(this);
        }
    }
    // Do the same thing on pointer up if we did not drag the operator
    public void OnPointerUp(PointerEventData data)
    {
        OnEndDrag(data);
    }
    // We only have this so that EndDrag actually works
    public void OnBeginDrag(PointerEventData data)
    {

    }
    // We only have this so that EndDrag actually works
    public void OnDrag(PointerEventData data)
    {
        // Do literally nothing at all
    }
    // When the source stops dragging, ask the matrix to confirm operation
    public void OnEndDrag(PointerEventData data)
    {
        // Attempt to confirm the operation
        MatrixParent.EndOperation();
    }
    #endregion
}
