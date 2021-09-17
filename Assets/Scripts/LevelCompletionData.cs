using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelCompletionData
{
    #region Public Properties
    public bool Completed => completed;
    public int FewestMoves => fewestMoves;
    public string FewestMovesString => completed ? fewestMoves.ToString() : "--";
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("True if the player has completed this level before")]
    private bool completed;
    [SerializeField]
    [Tooltip("Fewest moves it has taken the player to complete the level")]
    private int fewestMoves;
    #endregion

    #region Constructors
    public LevelCompletionData() : this(false, int.MaxValue) { }
    public LevelCompletionData(bool completed, int minimumMoves)
    {
        this.completed = completed;
        this.fewestMoves = minimumMoves;
    }
    public LevelCompletionData(LevelCompletionData other) : this(other.completed, other.fewestMoves) { }
    #endregion

    #region Public Methods
    public void CompleteLevel(int moves)
    {
        completed = true;
        fewestMoves = Mathf.Min(fewestMoves, moves);
    }
    #endregion
}