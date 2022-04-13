using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class MatrixMoveCountUI : MatrixUIChild
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Reference to the text that displays the current moves the player has made")]
    private TextMeshProUGUI currentMoveText;
    [SerializeField]
    [Tooltip("Text to display the fewest moves that the player has executed to beat this level")]
    private TextMeshProUGUI fewestMovesText;
    #endregion

    #region Public Methods
    public void UpdateText()
    {
        currentMoveText.text = "Moves: " + MatrixParent.CurrentMoves;
        currentMoveText.rectTransform.DOKill();
        currentMoveText.rectTransform.DOPunchScale(Vector3.one * UISettings.OperatorPunch, UISettings.OperatorPunchTime);
    }
    #endregion

    #region Monobehaviour Messages
    protected override void Start()
    {
        base.Start();

        // When operation finishes then update the current move text
        MatrixParent.OnMovesIncreased.AddListener(OnMovesIncreased);

        // Use the current level completion data to determine how to display the fewest moves that the player has solved the puzzle in
        LevelCompletionData completionData = GameplayManager.CurrentLevelCompletionData;
        fewestMovesText.text = "Fewest Moves: " + completionData.FewestMovesString;
    }
    #endregion

    #region Event Listeners
    public void OnMovesIncreased()
    {
        UpdateText();
    }
    #endregion
}
