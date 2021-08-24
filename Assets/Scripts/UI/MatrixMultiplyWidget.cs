using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    [Tooltip("Button that increases the scalar")]
    private Button increaseButton;
    [SerializeField]
    [Tooltip("Button that decreases the scalar")]
    private Button decreaseButton;
    [SerializeField]
    [Tooltip("Button that toggles if this is a multiplication or division")]
    private Button reciprocateButton;
    #endregion

    #region Private Data
    private Fraction scalar;
    private bool reciprocate = false;
    #endregion

    #region Messages
    protected override void Start()
    {
        base.Start();

        // Set the scalar to 1
        scalar = Fraction.one;
        UpdateDisplay();

        // Increment/decrement scalar when buttons are clicked
        increaseButton.onClick.AddListener(IncrementScalar);
        decreaseButton.onClick.AddListener(DecrementScalar);
        reciprocateButton.onClick.AddListener(ToggleReciprocal);

        // Setup the operation source to do a row scale, won't know destination until it is set on matrix ui
        operationSource.Setup(() => MatrixOperation.RowScale(-1, CurrentScalar));
    }
    private void IncrementScalar()
    {
        // Increment the scalar. If it is zero, move it past zero to 1/1
        scalar++;
        if (scalar == Fraction.zero) scalar = Fraction.one;
        // Play a sound!
        UpdateDisplay();
    }
    private void DecrementScalar()
    {
        // Decrement the scalar. If it is zero, move it past zero to -1/1
        scalar--;
        if (scalar == Fraction.zero) scalar = -Fraction.one;
        // Play a sound!
        UpdateDisplay();
    }
    private void ToggleReciprocal()
    {
        reciprocate = !reciprocate;
        // Play a sound!
        UpdateDisplay();
    }
    private void UpdateDisplay()
    {
        text.text = CurrentScalar.ToString();
    }
    #endregion
}
