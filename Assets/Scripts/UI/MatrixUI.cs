using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MatrixUI : MonoBehaviour
{
    #region Public Properties
    public Matrix CurrentMatrix => currentMatrix;
    public Matrix PreviewMatrix => previewMatrix;
    #endregion

    #region Private Properties
    // NOTE: you should only call this if both operationSource and operationDestination are non-null
    private MatrixOperation IntendedNextOperation
    {
        get
        {
            MatrixOperation operation = operationSource.Operation;
            operation.destinationRow = operationDestination.RowIndex;
            return operation;
        }
    }
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Reference to the prefab to instantiate for each matrix row")]
    private MatrixRowUI rowUIPrefab;
    [SerializeField]
    [Tooltip("Reference to the layout group used to hold all of the rows")]
    private RectTransform rowParent;
    #endregion

    #region Private Fields
    private MatrixRowUI[] rowUIs;

    private Matrix currentMatrix;
    private Matrix previewMatrix;

    private MatrixOperationSource operationSource;
    private MatrixRowUI operationDestination;
    #endregion

    #region Monobehaviour Messages
    private void Start()
    {
        // Get the data for this specific level based on the level's name
        string expectedDataName = SceneManager.GetActiveScene().name + "Data";
        LevelData data = Resources.Load<LevelData>(SceneManager.GetActiveScene().name + "Data");

        if(data)
        {
            // Initialize the UI
            currentMatrix = data.GetStartingMatrix();

            // Initialize the list of rows
            rowUIs = new MatrixRowUI[currentMatrix.rows];

            // Instantiate a row
            for(int i = 0; i < currentMatrix.rows; i++)
            {
                MatrixRowUI row = Instantiate(rowUIPrefab, rowParent);
                row.Setup(i);
                rowUIs[i] = row;
            }
        }
        else
        {
            Debug.LogWarning("MatrixUI: expected LevelData asset resource named '" + expectedDataName + "', but could not find any. " +
                "Make sure you have a LevelData asset with the name '" + expectedDataName + "' in a folder named 'Resources' somewhere in the project assets");
        }
    }
    #endregion

    #region Public Methods
    public void SetOperationSource(MatrixOperationSource operationSource)
    {
        this.operationSource = operationSource;

        // Play a sound!
        // Set the color of the operation source
        // Grey out the colors of all elements that don't apply for this operation
        // - row swap: all except the rows
        // - row scale: scalar arrows, row adder widgets
        // - row add: all scalar widgets and all other row adders
    }
    public void SetOperationDestination(MatrixRowUI operationDestination)
    {
        // Set the destination only if we have a source and the source row index is not the same as the destination row index
        // (prevents a self-swap and a self-add)
        if (operationSource && operationSource.Operation.destinationRow != operationDestination.RowIndex)
        {
            this.operationDestination = operationDestination;
            // Play a sound!
            // Set the color of the destination
            // Set the preview matrix and update all ui elements to display the preview
            previewMatrix = currentMatrix.Operate(IntendedNextOperation);
            ShowPreview();
        }
    }
    public void UnsetOperationDestination()
    {
        // Set the color of the current destination back to normal
        // Update all the ui elements to display the current matrix
        ShowCurrent();
        operationDestination = null;
    }

    public void ConfirmOperation()
    {
        // Check if operation destination is set or not
        if(operationDestination)
        {
            // Update the current matrix
            currentMatrix = currentMatrix.Operate(IntendedNextOperation);
            ShowCurrent();

            // Set the colors of all the ui elements back to normal
            // Play a sound!
            // Show a fun flash effect
        }
        else
        {
            // Set the colors of all ui elements back to normal
            // Play a sound!
        }

        // No more operation source or destination
        operationSource = null;
        operationDestination = null;
    }

    public void ShowCurrent()
    {
        foreach(MatrixRowUI row in rowUIs)
        {
            row.ShowCurrent();
        }
    }
    public void ShowPreview()
    {
        foreach(MatrixRowUI row in rowUIs)
        {
            row.ShowPreview();
        }
    }

    #endregion
}