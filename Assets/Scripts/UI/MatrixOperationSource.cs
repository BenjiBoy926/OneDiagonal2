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

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Rect transform to punch the scale of when the operation begins, and destination is set")]
    private ButtonEffects targetEffect;
    [SerializeField]
    [Tooltip("List of graphics to change color when this is used as the operation source")]
    private List<Graphic> graphics;
    #endregion

    #region Private Fields
    private Func<MatrixOperation> operationGetter = () => MatrixOperation.RowSwap(-1, -1);
    #endregion

    #region Public Methods
    public void Setup(Func<MatrixOperation> operationGetter)
    {
        Start();
        this.operationGetter = operationGetter;
        targetEffect.OperationSource = this;

        // Punch size each time that the destination is set, if this is the operation source
        MatrixParent.OnOperationDestinationSet.AddListener(() =>
        {
            if (IsCurrentOperationSource) 
                targetEffect.Play(
                    UISettings.GetOperatorColor(MatrixParent.IntendedNextOperationType),
                    ButtonSound.Preview);
        });

        // Start all graphics as disabled
        SetGraphicsActive(false);
    }
    #endregion

    #region Pointer Interface Implementations
    public void OnPointerDown(PointerEventData data)
    {
        if (!MatrixParent.OperationInProgress)
        {
            MatrixParent.SetOperationSource(this);

            // Set the color of the graphics this source is responsible for
            foreach (Graphic graphic in graphics)
            {
                graphic.enabled = true;
                graphic.color = UISettings.GetOperatorColor(Operation.type);
            }
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
        if (MatrixParent.OperationInProgress)
        {
            // Attempt to confirm the operation
            bool success = MatrixParent.ConfirmOperation();

            // If successful, use some fun effects
            if (success)
            {
                targetEffect.Play(
                    UISettings.GetOperatorColor(MatrixParent.IntendedNextOperationType), 
                    ButtonSound.NoSound);
            }

            // Disable graphics
            SetGraphicsActive(false);
        }
    }
    #endregion

    #region Private Methods
    private void SetGraphicsActive(bool active)
    {
        foreach(Graphic graphic in graphics)
        {
            graphic.enabled = active;
        }
    }
    #endregion
}
