using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonEffects : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    #region Private Properties
    private bool OperationInProgress => operationSource && operationSource.MatrixParent.OperationInProgress;
    /// <summary>
    /// Check if the next operation will work by checking if the matrix parent
    /// has an operation source and operation destination
    /// </summary>
    private bool OperationWillWork => operationSource && operationSource.MatrixParent.OperationSource && operationSource.MatrixParent.OperationDestination;
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Selectable object used for this effect")]
    private Selectable selectable;
    [SerializeField]
    [Tooltip("Type of effect to use for flashes and outlines")]
    private EffectType effectType;
    [SerializeField]
    [Tooltip("Color to use for the effects")]
    [FormerlySerializedAs("flashColor")]
    private Color effectColor = Color.cyan;
    [SerializeField]
    [Tooltip("Sound played when the pointer comes down on the button")]
    [FormerlySerializedAs("clickSound")]
    private ButtonSound pointerDownSound = ButtonSound.NoSound;
    [SerializeField]
    [Tooltip("Sound played when the pointer comes up on the button")]
    private ButtonSound pointerUpSound = ButtonSound.Confirm;

    [Header("Optional")]

    [SerializeField]
    [Tooltip("Referece to the operation source this creates effects for, if applicable")]
    private MatrixOperationSource operationSource;
    [SerializeField]
    [Tooltip("Reference to the row this creates effects for, if applicable")]
    private MatrixRowUI operationDestination;
    #endregion

    #region Private Fields
    private OutlineEffect currentOutline;
    private bool hasPointer;
    private bool isDragging;
    #endregion

    #region Public Methods
    public void Punch(ButtonSound sound)
    {
        UISettings.PlayButtonSound(sound);
        UISettings.PunchOperator(selectable.transform);
    }
    #endregion

    #region Pointer Interface Implementations
    public void OnPointerEnter(PointerEventData data)
    {
        // If selectable is interactable then
        // perform the effect
        if (selectable.interactable)
        {
            hasPointer = true;

            if (!isDragging)
            {
                // Setup the color to use for the flash
                Color color = effectColor;
                ButtonSound sound = ButtonSound.Hover;

                // If we have an operation source then instead set the color to the intended operation type color
                if (OperationInProgress)
                {
                    color = UISettings.GetOperatorColor(operationSource.MatrixParent.IntendedNextOperationType);
                    sound = ButtonSound.Preview;
                }

                // Create the pop effect for hovering
                Punch(sound);
                currentOutline = EffectsManager.FadeInOutline(transform, effectType, color);
            }
        }
    }
    public void OnPointerExit(PointerEventData data)
    {
        if (selectable.interactable)
        {
            hasPointer = false;

            // Wait for end of frame before trying to remove the outline
            // This is necessary because sometimes the pointer exits on the same frame
            // that the dragging begins, but exit will execute first. We need to wait
            // for the end of frame to see if dragging began in the same frame
            // before removing the outline
            StartCoroutine(RemoveOutlineOnEndOfFrame());
        }
    }
    public void OnPointerDown(PointerEventData data)
    {
        if (selectable.interactable)
        {
            // Create the pop effect for clicking
            EffectsManager.FadeOutOutline(transform, effectType, effectColor);
            Punch(pointerDownSound);
        }
    }
    public void OnPointerUp(PointerEventData data)
    {
        if (selectable.interactable && !isDragging)
        {
            // Create the pop effect for clicking
            EffectsManager.FadeOutOutline(transform, effectType, effectColor);
            Punch(pointerUpSound);
        }
    }
    public void OnBeginDrag(PointerEventData data)
    {
        if (selectable.interactable)
            isDragging = true;
    }
    public void OnDrag(PointerEventData data)
    {
        // This is just to make sure the other drag handlers work correctly
    }
    public void OnEndDrag(PointerEventData data)
    {
        if (selectable.interactable)
        {
            isDragging = false;

            // If we don't have the pointer then remove the current outline
            if (!hasPointer) currentOutline.FadeOut(currentOutline.Image.color);
            // If we do have the pointer then make some other outline fade out
            else EffectsManager.FadeOutOutline(transform, effectType, effectColor);

            // By default use the pointer up sound
            ButtonSound sound = pointerUpSound;

            // If the matrix has a valid operation then play the confirm sound instead
            if (operationSource && operationSource.MatrixParent.OperationIsValid)
                sound = ButtonSound.Confirm;
            
            // Punch the button with the pointer up sound
            Punch(sound);
        }
    }
    #endregion

    #region Private Methods
    private IEnumerator RemoveOutlineOnEndOfFrame()
    {
        yield return new WaitForEndOfFrame();

        // Fade out the outline if we are not dragging on this button
        if (!isDragging)
            currentOutline.FadeOut(currentOutline.Image.color);
    }
    #endregion
}
