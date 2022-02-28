using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Audio;

public class MenuButtonEffects : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
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
    [Tooltip("The object to enable/disable when the button is hovered")]
    private GameObject outline;
    [SerializeField]
    [Tooltip("Text to change color for when button is hovered")]
    private TextMeshProUGUI text;
    [SerializeField]
    [Tooltip("Color of the text when it is hovered")]
    private Color textHoverColor = Color.cyan;
    [SerializeField]
    [Tooltip("Sound to play when the button is hovered")]
    private AudioClip hoverClip;
    [SerializeField]
    [Tooltip("Sound to play when the button is clicked")]
    private AudioClip clickClip;
    #endregion

    #region Private Fields
    private Color defaultTextColor;
    #endregion

    #region Monobehaviour Messages
    private void Start()
    {
        outline.SetActive(false);
        defaultTextColor = text.color;
    }
    #endregion

    #region Pointer Interface Implementations
    public void OnPointerEnter(PointerEventData data)
    {
        // If selectable is interactable then
        // perform the effect
        if (selectable.interactable)
        {
            outline.SetActive(true);
            text.color = textHoverColor;

            // Play flash effect
            MatrixFlashEffect flashEffect = Instantiate(flashEffectPrefab, selectable.transform);
            flashEffect.Flash(flashColor);

            AudioManager.PlaySFX(hoverClip);
            UISettings.PunchOperator(selectable.transform);
        }
    }
    public void OnPointerExit(PointerEventData data)
    {
        outline.SetActive(false);
        text.color = defaultTextColor;
    }
    public void OnPointerClick(PointerEventData data)
    {
        if (selectable.interactable)
        {
            // Play flash effect
            MatrixFlashEffect flashEffect = Instantiate(flashEffectPrefab, selectable.transform);
            flashEffect.Flash(flashColor);

            AudioManager.PlaySFX(clickClip);
            UISettings.PunchOperator(selectable.transform);
        }
    }
    #endregion
}
