using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonEffects : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    #region Public Properties
    public MatrixOperationSource OperationSource
    {
        get;
        set;
    }
    #endregion

    #region Private Properties
    private bool OperationInProgress => OperationSource && OperationSource.MatrixParent.OperationInProgress;
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
    private ButtonSound clickSound = ButtonSound.Confirm;
    #endregion

    #region Public Methods
    public void Play(Color color, ButtonSound sound)
    {
        // Play flash effect
        MatrixFlashEffect flashEffect = Instantiate(flashEffectPrefab, selectable.transform);
        flashEffect.Flash(color);

        UISettings.PlayButtonSound(sound);
        UISettings.PunchOperator(selectable.transform);
    }
    #endregion

    #region Monobehaviour Messages
    private void Start()
    {
        // Try to get an operation source on this
        OperationSource = GetComponent<MatrixOperationSource>();
    }
    #endregion

    #region Pointer Interface Implementations
    public void OnPointerEnter(PointerEventData data)
    {
        // If selectable is interactable then
        // perform the effect
        if (selectable.interactable)
        {
            // Setup the color to use for the flash
            Color color = flashColor;
            ButtonSound sound = ButtonSound.Hover;

            // If we have an operation source then instead set the color to the intended operation type color
            if (OperationInProgress)
            {
                color = UISettings.GetOperatorColor(OperationSource.MatrixParent.IntendedNextOperationType);
                sound = ButtonSound.Preview;
            }

            // Create the pop effect for hovering
            Play(color, sound);
        }
    }
    public void OnPointerDown(PointerEventData data)
    {
        if (selectable.interactable)
        {
            // Create the pop effect for clicking
            Play(flashColor, clickSound);
        }
    }
    #endregion
}
