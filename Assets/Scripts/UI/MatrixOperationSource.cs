using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class MatrixOperationSource : MatrixUIChild, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
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
    private bool dragging = false;
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

        // Start all graphics as disabled
        SetGraphicsActive(false);
    }
    #endregion

    #region Pointer Interface Implementations
    public void OnPointerDown(PointerEventData data)
    {
        MatrixParent.SetOperationSource(this);

        // Set the color of the graphics this source is responsible for
        foreach (Graphic graphic in graphics)
        {
            graphic.enabled = true;
            graphic.color = UISettings.GetOperatorColor(Operation.type);
        }
        PunchSize();
    }
    // Do the same thing on pointer up if we did not drag the operator
    public void OnPointerUp(PointerEventData data)
    {
        if (!dragging) OnEndDrag(data);
    }
    // We only have this so that EndDrag actually works
    public void OnBeginDrag(PointerEventData data)
    {
        dragging = true;
    }
    // We only have this so that EndDrag actually works
    public void OnDrag(PointerEventData data)
    {
        // Do literally nothing at all
    }
    // When the source stops dragging, ask the matrix to confirm operation
    public void OnEndDrag(PointerEventData data)
    {
        bool success = MatrixParent.ConfirmOperation();
        if (success)
        {
            PunchSize();

            // Create a flash effect for each rect transform
            foreach(RectTransform rectTransform in rectTransforms)
            {
                Instantiate(UISettings.FlashEffect, rectTransform);
            }
        }

        // Disable graphics
        SetGraphicsActive(false);

        dragging = false;
    }
    #endregion

    #region Private Methods
    private void PunchSize()
    {
        foreach(RectTransform rt in rectTransforms)
        {
            UISettings.PunchOperator(rt);
        }
    }
    private void SetGraphicsActive(bool active)
    {
        foreach(Graphic graphic in graphics)
        {
            graphic.enabled = active;
        }
    }
    #endregion
}
