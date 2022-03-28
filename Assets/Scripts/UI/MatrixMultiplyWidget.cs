using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using AudioLibrary;

public class MatrixMultiplyWidget : MatrixUIChild
{
    #region Private Properties
    private Fraction CurrentScalar => reciprocate ? scalar.reciprocal : scalar;
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Reference to the script that sets the operation source on the matrix ui")]
    private MatrixOperationSource operationSource;
    [SerializeField]
    [Tooltip("Reference to the text used to display the scalar")]
    private TextMeshProUGUI text;
    [SerializeField]
    [Tooltip("Selectable object to drag to cause a matrix scaling")]
    private Selectable widget;

    [Space]

    [SerializeField]
    [Tooltip("Button that increases the scalar")]
    private RepeatButton increaseButton;
    [SerializeField]
    [Tooltip("Selectable component attached to the increase button")]
    private Selectable increaseSelectable;
    [SerializeField]
    [Tooltip("Sound that plays when the scalar increases")]
    private AudioClip increaseSound;

    [Space]

    [SerializeField]
    [Tooltip("Button that decreases the scalar")]
    private RepeatButton decreaseButton;
    [SerializeField]
    [Tooltip("Selectable component attached to the decrease button")]
    private Selectable decreaseSelectable;
    [SerializeField]
    [Tooltip("Sound that plays when the scalar decreases")]
    private AudioClip decreaseSound;

    [Space]

    [SerializeField]
    [Tooltip("Button that toggles if this is a multiplication or division")]
    private Button reciprocateButton;
    [SerializeField]
    [Tooltip("Sound that plays when the scalar is reciprocated")]
    private AudioClip reciprocateSound;
    #endregion

    #region Private Data
    private Fraction scalar;
    private bool reciprocate = false;
    #endregion

    #region Messages
    protected override void Start()
    {
        base.Start();

        // Set the scalar to 2
        scalar = Fraction.one + Fraction.one;
        OnScalarChanged();

        // Increment/decrement scalar when buttons are clicked
        increaseButton.RepeatAction.AddListener(IncrementScalar);
        decreaseButton.RepeatAction.AddListener(DecrementScalar);
        reciprocateButton.onClick.AddListener(ToggleReciprocal);

        // Setup the operation source to do a row scale, won't know destination until it is set on matrix ui
        operationSource.Setup(() => MatrixOperation.RowScale(-1, CurrentScalar));

        // Add listeners for the operation events
        MatrixParent.OnOperationStart.AddListener(OnMatrixOperationStarted);
        MatrixParent.OnOperationFinish.AddListener(OnMatrixOperationFinished);
    }
    private void IncrementScalar()
    {
        // Increment the scalar. If it is zero, move it past zero to 2/1
        scalar++;
        if (scalar == Fraction.zero) scalar = Fraction.one + Fraction.one;

        // Play a sound!
        AudioManager.PlaySFX(increaseSound);

        OnScalarChanged();
    }
    private void DecrementScalar()
    {
        // Decrement the scalar. If it is one, move it past one to -1/1
        scalar--;
        if (scalar == Fraction.one) scalar = -Fraction.one;

        // Play a sound!
        AudioManager.PlaySFX(decreaseSound);

        OnScalarChanged();
    }
    private void ToggleReciprocal()
    {
        reciprocate = !reciprocate;

        // Play a sound!
        AudioManager.PlaySFX(reciprocateSound);

        OnScalarChanged();
    }
    private void OnScalarChanged()
    {
        // Can only reciprocate a fraction that is not 1/1 or -1/1
        reciprocateButton.interactable = scalar > Fraction.one || scalar < -Fraction.one;
        if (!reciprocateButton.interactable) reciprocate = false;
        text.text = CurrentScalar.ToString();
    }
    private void OnMatrixOperationStarted()
    {
        widget.interactable = operationSource.IsCurrentOperationSource;
        increaseSelectable.interactable = false;
        decreaseSelectable.interactable = false;
        reciprocateButton.interactable = false;
    }
    private void OnMatrixOperationFinished(bool success)
    {
        widget.interactable = true;
        increaseSelectable.interactable = true;
        decreaseSelectable.interactable = true;
        reciprocateButton.interactable = true;
    }
    #endregion
}
