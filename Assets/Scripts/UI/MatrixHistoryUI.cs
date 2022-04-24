using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using NaughtyAttributes;

public class MatrixHistoryUI : MatrixUIChild
{
    #region Private Properties
    private bool UndoIsValid => history.Previous != null;
    private bool RedoIsValid => history.Next != null;
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Button the performs an Undo when clicked")]
    private EventHandler undoButton;
    [SerializeField]
    [Tooltip("Button that performs a Redo when clicked")]
    private EventHandler redoButton;
    [SerializeField]
    [Tooltip("Matrix history")]
    [ReadOnly]
    [AllowNesting]
    private MatrixHistory history = new MatrixHistory();
    #endregion

    #region Private Fields
    private MatrixOperationUI operationUI;
    #endregion

    #region Public Methods
    public bool Undo()
    {
        // Try to undo
        bool success = history.Undo();

        // If undo succeeds then update the matrix
        if (success)
        {
            MatrixParent.CurrentMatrix = history.Current.Matrix;
            MatrixParent.IncreaseMovesMade();
            OnHistoryUpdate();
            AttemptUndoPreview();
        }

        return success;
    }
    public bool Redo()
    {
        // Try to redo
        bool success = history.Redo();
        
        // If redo succeeds then update the matrix
        if (success)
        {
            MatrixParent.CurrentMatrix = history.Current.Matrix;
            MatrixParent.IncreaseMovesMade();
            OnHistoryUpdate();
            AttemptRedoPreview();
        }

        return success;
    }
    public void AttemptUndoPreview()
    {
        if (UndoIsValid && undoButton.IsPointerPresent)
        {
            MatrixParent.PreviewMatrix = history.Previous.Matrix;
            MatrixParent.HighlightOperationParticipants(history.Current.PreviousOperation);
            operationUI.ShowText(history.Current.PreviousOperation, "UNDO: {0}");
        }
        else ClearPreview();
    }
    public void AttemptRedoPreview()
    {
        if (RedoIsValid && redoButton.IsPointerPresent)
        {
            MatrixParent.PreviewMatrix = history.Next.Matrix;
            MatrixParent.HighlightOperationParticipants(history.Next.PreviousOperation);
            operationUI.ShowText(history.Next.PreviousOperation, "REDO: {0}");
        }
        else ClearPreview();
    }
    public void ClearPreview()
    {
        MatrixParent.ClearOperationParticipantHighlights();
        MatrixParent.ShowCurrent();
        operationUI.HideText();
    }
    #endregion

    #region Monobehaviour Messages
    protected override void OnEnable()
    {
        base.OnEnable();

        undoButton.PointerEnterEvent += AttemptUndoPreviewCallback;
        undoButton.PointerExitEvent += ClearPreviewCallback;
        undoButton.PointerClickEvent += UndoCallback;

        redoButton.PointerEnterEvent += AttemptRedoPreviewCallback;
        redoButton.PointerExitEvent += ClearPreviewCallback;
        redoButton.PointerClickEvent += RedoCallback;
    }
    protected override void OnDisable()
    {
        base.OnDisable();

        undoButton.PointerEnterEvent -= AttemptUndoPreviewCallback;
        undoButton.PointerExitEvent -= ClearPreviewCallback;
        undoButton.PointerClickEvent -= UndoCallback;

        redoButton.PointerEnterEvent -= AttemptRedoPreviewCallback;
        redoButton.PointerExitEvent -= ClearPreviewCallback;
        redoButton.PointerClickEvent -= RedoCallback;
    }
    protected override void Start()
    {
        base.Start();

        MatrixParent.OnOperationFinish.AddListener(OnOperationFinish);
        history.Insert(new MatrixHistoryItem(MatrixParent.CurrentMatrix));
        operationUI = MatrixParent.GetComponentInChildren<MatrixOperationUI>(true);

        OnHistoryUpdate();
    }
    private void Update()
    {
        // The 'Z' key initiates undos and redos
        if (Input.GetKeyDown(KeyCode.Z))
        {
            // Check if a control key is pressed
            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            {
                bool operationPerformed;

                // If shift is also pressed it should be a redo
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                    operationPerformed = Redo();
                // No shifts means undo
                else operationPerformed = Undo();

                // Play the flip sound for the event
                if (operationPerformed)
                    UISettings.PlayButtonSound(ButtonSound.Flip);
            }
        }
    }
    #endregion

    #region Event Listeners
    private void UndoCallback(PointerEventData data) => Undo();
    private void RedoCallback(PointerEventData data) => Redo();
    private void AttemptUndoPreviewCallback(PointerEventData data) => AttemptUndoPreview();
    private void AttemptRedoPreviewCallback(PointerEventData data) => AttemptRedoPreview();
    private void ClearPreviewCallback(PointerEventData data) => ClearPreview();
    /// <summary>
    /// Whenever an operation is a success, insert the current matrix
    /// into the history
    /// </summary>
    /// <param name="success"></param>
    private void OnOperationFinish(bool success)
    {
        if (success)
        {
            history.Insert(new MatrixHistoryItem(MatrixParent.CurrentMatrix, MatrixParent.IntendedNextOperation));
            OnHistoryUpdate();
        }
    }
    #endregion

    #region Private Methods
    private void OnHistoryUpdate()
    {
        undoButton.interactable = UndoIsValid;
        redoButton.interactable = RedoIsValid;
    }
    #endregion
}
