using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MatrixHistory
{
    #region Public Properties
    public MatrixHistoryItem Previous => GetOrNull(position - 1);
    public MatrixHistoryItem Current => GetOrNull(position);
    public MatrixHistoryItem Next => GetOrNull(position + 1);
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("List of states in the history")]
    private List<MatrixHistoryItem> states = new List<MatrixHistoryItem>();
    [SerializeField]
    [Tooltip("Current position of the history")]
    private int position = -1;
    #endregion

    #region Public Methods
    public void Insert(MatrixHistoryItem matrix)
    {
        int nextPosition = position + 1;

        // If position is in range of the states then remove all the states
        // after the position
        if (nextPosition >= 0 && nextPosition < states.Count)
        {
            states.RemoveRange(nextPosition, states.Count - nextPosition);
        }

        states.Add(matrix);
        position++;
    }
    public bool Undo()
    {
        bool success = position > 0;
        if (success)
            position--;
        return success;
    }
    public bool Redo()
    {
        bool success = position < (states.Count - 1);
        if (success)
            position++;
        return success;
    }
    #endregion

    #region Private Methods
    public MatrixHistoryItem GetOrNull(int index)
    {
        if (index >= 0 && index < states.Count) return states[index];
        else return null;
    }
    #endregion
}
