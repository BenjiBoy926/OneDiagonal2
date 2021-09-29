using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class OptionalConfirmButton : MonoBehaviour
{
    #region Public Properties
    public UnityEvent OnConfirm => onConfirm;
    #endregion

    #region Protected Editor Fields
    [SerializeField]
    [Tooltip("Button that performs an action when clicked")]
    protected Button actionButton;
    [SerializeField]
    [Tooltip("If true then the action needs confirmation to be performed")]
    protected bool requireConfirmation;
    [SerializeField]
    [Tooltip("Reference to the transform that the window will be instantiated under")]
    protected Transform windowParent;
    [SerializeField]
    [TextArea(3, 10)]
    [Tooltip("Message to put in the window that asks for confirmation")]
    protected string confirmationMessage = "Are you sure that you want to ...?";
    [SerializeField]
    [Tooltip("Event invoked when the button action runs")]
    private UnityEvent onConfirm;
    #endregion

    #region Protected Fields
    protected GenericYesNoWindow confirmationWindow;
    #endregion

    #region Monobehaviour Messages
    protected virtual void Start()
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
            confirmationWindow.Open(confirmationMessage);
            confirmationWindow.SetResponse(GenericYesNoWindow.ResponseType.Yes, ButtonAction);
        }
        // If confirmation is not required then perform the action immediately
        else if (!requireConfirmation) ButtonAction();
    }
    protected virtual void ButtonAction()
    {
        onConfirm.Invoke();
    }
    #endregion
}
