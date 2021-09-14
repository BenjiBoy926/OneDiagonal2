using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelCompletionData
{
    #region Public Properties
    public bool Completed => completed;
    public int MinimumMoves => minimumMoves;
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("True if the player has completed this level before")]
    private bool completed;
    [SerializeField]
    [Tooltip("Number of moves it took the player to complete the level")]
    private int minimumMoves;
    #endregion

    #region Constructors
    public LevelCompletionData() : this(false, int.MaxValue) { }
    public LevelCompletionData(bool completed, int minimumMoves)
    {
        this.completed = completed;
        this.minimumMoves = minimumMoves;
    }
    #endregion

    #region Public Methods
    public void CompleteLevel(int moves)
    {
        completed = true;
        minimumMoves = Mathf.Min(minimumMoves, moves);
    }
    #endregion
}
