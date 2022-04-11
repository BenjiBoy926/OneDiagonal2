using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MatrixHistory
{
    #region Public Properties
    public Matrix Previous => GetOrNull(position - 1);
    public Matrix Current => GetOrNull(position);
    public Matrix Next => GetOrNull(position + 1);
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("List of states in the history")]
    private List<Matrix> states = new List<Matrix>();
    [SerializeField]
    [Tooltip("Current position of the history")]
    private int position = -1;
    #endregion

    #region Public Methods
    public void Insert(Matrix matrix)
    {
        // If position is in range of the states then remove all the states
        // after the position
        if (position >= 0 && position < states.Count)
        {
            states.RemoveRange(position + 1, states.Count - position);
        }

        states.Add(matrix);
        position++;
    }
    public void Undo()
    {
        position--;
    }
    public void Redo()
    {
        position++;
    }
    #endregion

    #region Private Methods
    public Matrix GetOrNull(int index)
    {
        if (index >= 0 && index < states.Count) return states[index];
        else return null;
    }
    #endregion
}
