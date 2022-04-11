using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    #region Public Methods
    public void Undo()
    {
        // Try to undo
        bool success = history.Undo();

        // If undo succeeds then update the matrix
        if (success)
        {
            MatrixParent.CurrentMatrix = history.Current;
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
            MatrixParent.CurrentMatrix = history.Current;
            OnHistoryUpdate();
        }
    }
    #endregion

    #region Monobehaviour Messages
    protected override void Start()
    {
        base.Start();
        MatrixParent.OnOperationFinish.AddListener(OnOperationFinish);
        history.Insert(MatrixParent.CurrentMatrix);
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
            history.Insert(MatrixParent.CurrentMatrix);
            OnHistoryUpdate();
        }
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
