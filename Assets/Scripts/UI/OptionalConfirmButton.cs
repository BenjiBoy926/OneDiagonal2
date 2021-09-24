using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class OptionalConfirmButton : MonoBehaviour
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Button that performs an action when clicked")]
    private Button actionButton;
    [SerializeField]
    [Tooltip("If true then the action needs confirmation to be performed")]
    private bool requireConfirmation;
    [SerializeField]
    [Tooltip("Reference to the transform that the window will be instantiated under")]
    private Transform windowParent;
    [SerializeField]
    [TextArea(3, 10)]
    [Tooltip("Message to put in the window that asks for confirmation")]
    private string confirmationMessage = "Are you sure that you want to ...?";
    #endregion

    #region Private Fields
    private GenericYesNoWindow confirmationWindow;
    #endregion

    #region Monobehaviour Messages
    private void Start()
    {
        actionButton.onClick.AddListener(OnButtonClicked);
    }
    #endregion

    #region Private/Protected Methods
    private void OnButtonClicked()
    {
        // If confirmation is required and no window exists then create one
        if (requireConfirmation && !confirmationWindow)
        {
            confirmationWindow = GenericYesNoWindow.InstantiateFromResource(windowParent);
            confirmationWindow.Setup(confirmationMessage, ButtonAction, null, null);
        }
        // If confirmation is not required then perform the action immediately
        else if (!requireConfirmation) ButtonAction();
    }
    protected abstract void ButtonAction();
    #endregion
}
