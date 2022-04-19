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
    private Button undoButton;
    [SerializeField]
    [Tooltip("Button that performs a Redo when clicked")]
    private Button redoButton;
    [SerializeField]
    [Tooltip("Matrix history")]
    [ReadOnly]
    [AllowNesting]
    private MatrixHistory history = new MatrixHistory();
    #endregion

    #region Private Fields
    private EventTrigger undoTrigger;
    private EventTrigger redoTrigger;
    #endregion

    #region Public Methods
    public bool Undo()
    {
        // Try to undo
        bool success = history.Undo();

        // If undo succeeds then update the matrix
        if (success)
        {
            ApplyHistoryToMatrix();
            OnHistoryUpdate();
            AttemptUndoPreview(new BaseEventData(EventSystem.current));
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
            ApplyHistoryToMatrix();
            OnHistoryUpdate();
            AttemptRedoPreview(new BaseEventData(EventSystem.current));
        }

        return success;
    }
    #endregion

    #region Monobehaviour Messages
    protected override void Start()
    {
        base.Start();
        MatrixParent.OnOperationFinish.AddListener(OnOperationFinish);
        history.Insert(new MatrixHistoryItem(MatrixParent.CurrentMatrix));

        undoTrigger = undoButton.gameObject.GetOrAddComponent<EventTrigger>();
        undoTrigger.AddTrigger(EventTriggerType.PointerEnter, AttemptUndoPreview);
        undoTrigger.AddTrigger(EventTriggerType.PointerExit, ClearPreview);

        redoTrigger = redoButton.gameObject.GetOrAddComponent<EventTrigger>();
        redoTrigger.AddTrigger(EventTriggerType.PointerEnter, AttemptRedoPreview);
        redoTrigger.AddTrigger(EventTriggerType.PointerExit, ClearPreview);

        OnHistoryUpdate();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        undoButton.onClick.AddListener(UndoCallback);
        redoButton.onClick.AddListener(RedoCallback);
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        undoButton.onClick.RemoveListener(UndoCallback);
        redoButton.onClick.RemoveListener(RedoCallback);
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
    private void UndoCallback() => Undo();
    private void RedoCallback() => Redo();
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
    private void AttemptUndoPreview(BaseEventData data)
    {
        if (UndoIsValid)
        {
            MatrixParent.PreviewMatrix = history.Previous.Matrix;
            MatrixParent.HighlightOperationParticipants(history.Current.PreviousOperation);
        }
    }
    private void AttemptRedoPreview(BaseEventData data)
    {
        if (RedoIsValid)
        {
            MatrixParent.PreviewMatrix = history.Next.Matrix;
            MatrixParent.HighlightOperationParticipants(history.Next.PreviousOperation);
        }
    }
    private void ClearPreview(BaseEventData data)
    {
        MatrixParent.ClearOperationParticipantHighlights();
        MatrixParent.ShowCurrent();
    }
    #endregion

    #region Private Methods
    private void ApplyHistoryToMatrix()
    {
        MatrixParent.CurrentMatrix = history.Current.Matrix;
        MatrixParent.IncreaseMovesMade();
        MatrixParent.ClearOperationParticipantHighlights();
    }
    private void OnHistoryUpdate()
    {
        undoButton.interactable = UndoIsValid;
        redoButton.interactable = RedoIsValid;
    }
    #endregion
}
