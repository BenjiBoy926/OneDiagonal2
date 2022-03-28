using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonEffects : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    #region Private Properties
    private bool OperationInProgress => operationSource && operationSource.MatrixParent.OperationInProgress;
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Selectable object used for this effect")]
    private Selectable selectable;
    [SerializeField]
    [Tooltip("Flash effect used to flash when the button is clicked")]
    private MatrixFlashEffect flashEffectPrefab;
    [SerializeField]
    [Tooltip("Color to use for the flash effect")]
    private Color flashColor = Color.cyan;
    [SerializeField]
    [Tooltip("If true, then the button will play the confirm sound when clicked")]
    private bool playConfirmSoundOnClick = true;
    #endregion

    #region Private Fields
    private MatrixOperationSource operationSource;
    #endregion

    #region Monobehaviour Messages
    private void Start()
    {
        // Try to get an operation source on this
        operationSource = GetComponent<MatrixOperationSource>();
    }
    #endregion

    #region Pointer Interface Implementations
    public void OnPointerEnter(PointerEventData data)
    {
        // If selectable is interactable then
        // perform the effect
        if (selectable.interactable && !OperationInProgress)
        {
            // Play flash effect
            MatrixFlashEffect flashEffect = Instantiate(flashEffectPrefab, selectable.transform);
            flashEffect.Flash(flashColor);

            UISettings.PlayButtonSound(ButtonSound.Hover);
            UISettings.PunchOperator(selectable.transform);
        }
    }
    public void OnPointerDown(PointerEventData data)
    {
        if (selectable.interactable && !operationSource)
        {
            // Play flash effect
            MatrixFlashEffect flashEffect = Instantiate(flashEffectPrefab, selectable.transform);
            flashEffect.Flash(flashColor);

            if (playConfirmSoundOnClick)
            {
                UISettings.PlayButtonSound(ButtonSound.Confirm);
            }
            UISettings.PunchOperator(selectable.transform);
        }
    }
    #endregion
}
