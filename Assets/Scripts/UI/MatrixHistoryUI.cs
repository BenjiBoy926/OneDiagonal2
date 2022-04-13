using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MatrixHistoryUI : MatrixUIChild
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Button the performs an Undo when clicked")]
    private Button undoButton;
    [SerializeField]
    [Tooltip("Button that performs a Redo when clicked")]
    private Button redoButton;
    [SerializeField]
    [Tooltip("Matrix history")]
    private MatrixHistory history = new MatrixHistory();
    #endregion

    #region Private Fields
    private EventTrigger undoTrigger;
    private EventTrigger redoTrigger;
    #endregion

    #region Public Methods
    public void Undo()
    {
        // Try to undo
        bool success = history.Undo();

        // If undo succeeds then update the matrix
        if (success)
        {
            MatrixParent.CurrentMatrix = history.Current.Matrix;
            MatrixParent.IncreaseMovesMade();

            // Flash the operators that participated in the undone operation

            OnHistoryUpdate();
        }
    }
    public void Redo()
    {
        // Try to redo
        bool success = history.Redo();
        
        // If redo succeeds then update the matrix
        if (success)
        {
            MatrixParent.CurrentMatrix = history.Current.Matrix;
            MatrixParent.IncreaseMovesMade();

            // Flash the operators that participated in the redone operation

            OnHistoryUpdate();
        }
    }
    #endregion

    #region Monobehaviour Messages
    protected override void Start()
    {
        base.Start();
        MatrixParent.OnOperationFinish.AddListener(OnOperationFinish);
        history.Insert(new MatrixHistoryItem(MatrixParent.CurrentMatrix));

        undoTrigger = undoButton.gameObject.GetOrAddComponent<EventTrigger>();
        undoTrigger.AddTrigger(EventTriggerType.PointerEnter, OnUndoButtonPointerEnter);
        undoTrigger.AddTrigger(EventTriggerType.PointerExit, OnButtonPointerExit);

        redoTrigger = redoButton.gameObject.GetOrAddComponent<EventTrigger>();
        redoTrigger.AddTrigger(EventTriggerType.PointerEnter, OnRedoButtonPointerEnter);
        redoTrigger.AddTrigger(EventTriggerType.PointerExit, OnButtonPointerExit);

        OnHistoryUpdate();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        undoButton.onClick.AddListener(Undo);
        redoButton.onClick.AddListener(Redo);
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        undoButton.onClick.RemoveListener(Undo);
        redoButton.onClick.RemoveListener(Redo);
    }
    private void Update()
    {
        // The 'Z' key initiates undos and redos
        if (Input.GetKeyDown(KeyCode.Z))
        {
            // Check if a control key is pressed
            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            {
                // If shift is also pressed it should be a redo
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                    Redo();
                // No shifts means undo
                else Undo();

                // Play the flip sound for the event
                UISettings.PlayButtonSound(ButtonSound.Flip);
            }
        }
    }
    #endregion

    #region Event Listeners
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
    private void OnUndoButtonPointerEnter(BaseEventData data)
    {
        if (undoButton.interactable)
            MatrixParent.HighlightOperationParticipants(history.Current.PreviousOperation);
 
    }
    private void OnRedoButtonPointerEnter(BaseEventData data)
    {
        if (redoButton.interactable)
            MatrixParent.HighlightOperationParticipants(history.Next.PreviousOperation);
    }
    private void OnButtonPointerExit(BaseEventData data)
    {
        MatrixParent.ClearOperationParticipantHighlights();
    }
    #endregion

    #region Private Methods
    private void OnHistoryUpdate()
    {
        undoButton.interactable = history.Previous != null;
        redoButton.interactable = history.Next != null;
    }
    #endregion
}
