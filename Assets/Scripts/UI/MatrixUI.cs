using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using AudioUtility;

public class MatrixUI : MonoBehaviour
{
    #region Public Properties
    public MatrixRowUI[] RowUIs => rowUIs;
    public Matrix CurrentMatrix
    {
        get => currentMatrix;
        set
        {
            currentMatrix = value;
            ShowCurrent();
        }
    }
    public Matrix PreviewMatrix
    {
        get => previewMatrix;
        set
        {
            previewMatrix = value;
            ShowPreview();
        }
    }
    public int Rows => CurrentMatrix.rows;
    public int Cols => CurrentMatrix.cols;
    public MatrixOperationSource OperationSource => operationSource;
    public MatrixRowUI OperationDestination => operationDestination;
    public UnityEvent OnOperationStart => onOperationStart;
    public UnityEvent OnOperationDestinationSet => onOperationDestinationSet;
    public UnityEvent OnOperationDestinationUnset => onOperationDestinationUnset;
    public UnityEvent<bool> OnOperationFinish => onOperationFinish;
    public UnityEvent OnMovesIncreased => onMovesIncreased;
    public UnityEvent OnMatrixSolved => onMatrixSolved;
    // NOTE: you should only call this if the operations source is non-null
    public MatrixOperation.Type IntendedNextOperationType => operationSource.Operation.type;
    public MatrixOperation IntendedNextOperation
    {
        get
        {
            if (operationSource)
            {
                MatrixOperation operation = operationSource.Operation;

                // If a destination exists then set it
                if (operationDestination)
                {
                    operation.destinationRow = operationDestination.RowIndex;
                }

                return operation;
            }
            else return MatrixOperation.Invalid;
        }
    }
    public bool OperationIsValid => operationSource && operationDestination;
    public int CurrentMoves => currentMoves;
    public bool OperationInProgress => operationInProgress;
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Canvas group that controls the interactability of all children in the matrix")]
    private CanvasGroup canvasGroup;
    [SerializeField]
    [Tooltip("Reference to the prefab to instantiate for each matrix row")]
    private MatrixRowUI rowUIPrefab;
    [SerializeField]
    [Tooltip("Reference to the layout group used to hold all of the rows")]
    private RectTransform rowParent;
    [SerializeField]
    [Tooltip("Object used to create a fun highlight effect when the player solves the puzzle")]
    private MatrixDiagonalHighlightEffect highlightPrefab;

    [Space]

    [SerializeField]
    [Tooltip("Sound that plays when an operation is cancelled")]
    private AudioClip operationCancelSound;
    [SerializeField]
    [Tooltip("Sound that plays when the player previews an operation that would result in them solving the puzzle")]
    private AudioClip previewIdentitySound;
    [SerializeField]
    [Tooltip("Sound that plays when the matrix is solved")]
    private AudioClip matrixSolveSound;

    [Space]

    [SerializeField]
    [Tooltip("Event invoked when the matrix begins an operation")]
    private UnityEvent onOperationStart;
    [SerializeField]
    [Tooltip("Event invoked when the operation source is set")]
    private UnityEvent onOperationDestinationSet;
    [SerializeField]
    [Tooltip("Event invoked when the operation destination is unset")]
    private UnityEvent onOperationDestinationUnset;
    [SerializeField]
    [Tooltip("Event invoked when the matrix finishes an operation")]
    private UnityEvent<bool> onOperationFinish;
    [SerializeField]
    [Tooltip("Event invoked when the number of moves made on the matrix increases")]
    private UnityEvent onMovesIncreased;
    [SerializeField]
    [Tooltip("Event invoked when the matrix is solved")]
    private UnityEvent onMatrixSolved;
    #endregion

    #region Private Fields
    private bool operationInProgress;
    private MatrixRowUI[] rowUIs = new MatrixRowUI[0];

    private Matrix currentMatrix;
    private Matrix previewMatrix;

    private MatrixOperationSource operationSource;
    private MatrixRowUI operationDestination;

    private MatrixDiagonalHighlightEffect currentHighlight;
    private OutlineEffect operationSourceOutline;
    private OutlineEffect operationDestinationOutline;

    // Current number of operations the player has performed on the matrix
    private int currentMoves = 0;
    #endregion

    #region Public Methods
    public void Setup(LevelData currentLevelData)
    {
        // Make sure all elements block raycasts
        canvasGroup.blocksRaycasts = true;

        // Get the starting matrix of the current level data
        currentMatrix = currentLevelData.GetStartingMatrix();

        // If we have a highlight active then destroy it
        if (currentHighlight) Destroy(currentHighlight.gameObject);

        // Destroy any existing rows
        foreach(MatrixRowUI row in rowUIs)
        {
            Destroy(row.gameObject);
        }

        // Initialize the list of rows
        rowUIs = new MatrixRowUI[currentMatrix.rows];

        // Instantiate a row
        for (int i = 0; i < currentMatrix.rows; i++)
        {
            MatrixRowUI row = Instantiate(rowUIPrefab, rowParent);
            row.Setup(i);
            rowUIs[i] = row;
        }

        // Force the row parent to rebuild now that new rows are in it
        LayoutRebuilder.ForceRebuildLayoutImmediate(rowParent);
    }
    public void StartOperation(MatrixOperationSource operationSource)
    {
        this.operationSource = operationSource;
        operationInProgress = true;
        operationDestination = null;

        // Invoke the operation finish event
        onOperationStart.Invoke();
    }
    public bool SetOperationDestination(MatrixRowUI operationDestination)
    {
        bool success = operationInProgress && operationSource && operationSource.Operation.sourceRow != operationDestination.RowIndex;

        // Set the destination only if we have a source and the source row index is not the same as the destination row index
        // (prevents a self-swap and a self-add)
        if (success)
        {
            this.operationDestination = operationDestination;

            // Set the color of the destination
            // Set the preview matrix and update all ui elements to display the preview
            previewMatrix = currentMatrix.Operate(IntendedNextOperation);
            ShowPreview();

            // If we are previewing the identity then play the sound
            if(previewMatrix.isIdentity)
            {
                AudioManager.PlaySFX(previewIdentitySound);
            }

            // Invoke the event for the destination set
            onOperationDestinationSet.Invoke();
        }

        return success;
    }
    public void UnsetOperationDestination()
    {
        if (operationInProgress && operationDestination)
        {
            // Set the color of the current destination back to normal
            // Update all the ui elements to display the current matrix
            ShowCurrent();
            operationDestination = null;

            // Invoke the event
            onOperationDestinationUnset.Invoke();
        }
    }

    public bool FinishOperation()
    {
        if (operationInProgress)
        {
            bool operationSuccess = operationDestination;

            // Check if operation destination is set or not
            if (operationSuccess)
            {
                // Update the current matrix and current moves
                currentMatrix = currentMatrix.Operate(IntendedNextOperation);

                ShowCurrent();

                // Play a sound!
                UISettings.PlayButtonSound(ButtonSound.Confirm);

                // If current matrix is the identity, then invoke the matrix solved event
                if (currentMatrix.isIdentity)
                {
                    // None of the children block raycasts now that the matrix is solved
                    canvasGroup.blocksRaycasts = false;
                    // Play a sound!
                    AudioManager.PlaySFX(matrixSolveSound);
                    // Create the highlight effect
                    currentHighlight = Instantiate(highlightPrefab, transform);
                    // Invoke the public event
                    onMatrixSolved.Invoke();
                }

                // Increase the moves made
                IncreaseMovesMade();
            }
            else
            {
                // Play a sound!
                AudioManager.PlaySFX(operationCancelSound);
            }

            // Operation no longer in progress
            operationInProgress = false;

            // Invoke operation finished event
            onOperationFinish.Invoke(operationSuccess);

            return operationSuccess;
        }
        else return false;
    }
    public void IncreaseMovesMade()
    {
        currentMoves++;
        onMovesIncreased.Invoke();
    }
    public void HighlightOperationParticipants(MatrixOperation operation)
    {
        ClearOperationParticipantHighlights();

        Color color = UISettings.GetOperatorColor(operation.type);
        Transform source = null;
        OutlineType sourceType = OutlineType.Rect;

        switch(operation.type)
        {
            case MatrixOperation.Type.Swap:
                source = rowUIs[operation.sourceRow].transform;
                sourceType = OutlineType.Rect;
                break;
            case MatrixOperation.Type.Scale:
                source = GetComponentInChildren<MatrixMultiplyWidget>(true).Widget.transform;
                sourceType = OutlineType.Circle;
                break;
            case MatrixOperation.Type.Add:
                source = rowUIs[operation.sourceRow]
                    .GetComponentInChildren<MatrixRowAddWidget>(true)
                    .transform;
                sourceType = OutlineType.TriangleDown;
                break;
        }
        operationSourceOutline = OutlineManager.FadeInOutline(source, sourceType, color);

        MatrixRowUI destination = rowUIs[operation.destinationRow];
        operationDestinationOutline = OutlineManager.FadeInOutline(destination.transform, OutlineType.Rect, color);
    }
    public void ClearOperationParticipantHighlights()
    {
        if (operationSourceOutline && !operationSourceOutline.IsRemoved)
            operationSourceOutline.FadeOut();

        if (operationDestinationOutline && !operationDestinationOutline.IsRemoved)
            operationDestinationOutline.FadeOut();
    }
    public void ShowCurrent()
    {
        foreach (MatrixRowUI row in rowUIs)
        {
            row.ShowCurrent();
        }
    }
    public void ShowPreview()
    {
        foreach (MatrixRowUI row in rowUIs)
        {
            row.ShowPreview();
        }
    }
    #endregion
}
