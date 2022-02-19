using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;
using Audio;

public class MatrixSolvedUI : MatrixUIChild
{
    #region Public Typedefs
    [System.Serializable]
    public class MovesText
    {
        public RectTransform textRoot;
        public TextMeshProUGUI text;
    }
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("The root game object for all of the UI")]
    private GameObject uiRoot;
    [SerializeField]
    [Tooltip("Image to fade in the matrix solved UI")]
    private Image panel;
    [SerializeField]
    [Tooltip("Text used to congratulate the player")]
    private TextMeshProUGUI congratsText;
    [SerializeField]
    [Tooltip("Text that shows the player the number of moves it took to win")]
    private MovesText movesText;
    [SerializeField]
    [Tooltip("Text that shows the player the fewest number of moves it took them to solve the puzzle last time")]
    private MovesText fewestMovesText;
    [SerializeField]
    [Tooltip("Object that shows additional congratulatory text if the player gets a new high score")]
    private GameObject highScoreCongratsObject;
    [SerializeField]
    [Tooltip("Button that sends the player back to main menu")]
    private GameObject mainMenuButton;
    [SerializeField]
    [Tooltip("Button that sends the player to the next stage")]
    private GameObject advanceButton;
    [SerializeField]
    [Tooltip("Button that replays the current level")]
    private GameObject replayButton;

    [Space]

    [SerializeField]
    [Tooltip("Time to wait after the matrix is solved before starting to show this UI")]
    private float startWait = 2;
    [SerializeField]
    [Tooltip("Time to wait between starting to show the UI and showing the congrats text")]
    private float congratsWait = 1;
    [SerializeField]
    [Tooltip("Time to wait between showing congrats text and showing moves text")]
    private float movesTextWait = 1;
    [SerializeField]
    [Tooltip("Time to wait between showing the moves text and showing the fewest moves text")]
    private float fewestMovesTextWait = 0.5f;
    [SerializeField]
    [Tooltip("Time to wait between showing the fewest moves text and updating the fewest moves text")]
    private float fewestMovesTextUpdateWait = 1f;

    [Space]

    [SerializeField]
    [Tooltip("Audio clip played when each object is revealed in the UI")]
    private AudioClip revealSound;
    [SerializeField]
    [Tooltip("Audio that plays when player got a new high score")]
    private AudioClip newHighScoreSound;
    #endregion

    #region Private Fields
    // The completion state of the level when we first entered it
    private LevelCompletionData completionDataOnPuzzleStart;
    #endregion

    #region Monobehaviour Messages
    protected override void Start()
    {
        base.Start();

        // Disable the root
        uiRoot.SetActive(false);

        // Disable all of the items that are revealed at the end
        panel.enabled = false;
        panel.color = Color.clear;
        congratsText.enabled = false;
        movesText.textRoot.gameObject.SetActive(false);
        fewestMovesText.textRoot.gameObject.SetActive(false);
        highScoreCongratsObject.SetActive(false);
        mainMenuButton.SetActive(false);
        advanceButton.SetActive(false);
        replayButton.SetActive(false);

        // Create a copy of the current level completion data at the start
        completionDataOnPuzzleStart = new LevelCompletionData(GameplayManager.CurrentLevelCompletionData);

        // When the matrix is solved then start the routine
        MatrixParent.OnMatrixSolved.AddListener(() => StartCoroutine(MatrixSolvedUIRoutine()));
    }
    #endregion

    #region Private Methods
    private IEnumerator MatrixSolvedUIRoutine()
    {
        // Enable the root again
        uiRoot.SetActive(true);
        yield return new WaitForSeconds(startWait);

        // Change color of the panel and wait for completion
        panel.enabled = true;
        yield return panel.DOColor(new Color(0f, 0f, 0f, 0.8f), congratsWait)
            .WaitForCompletion();

        // Show the buttons
        mainMenuButton.SetActive(true);
        advanceButton.SetActive(true);
        replayButton.SetActive(true);

        // Enable the text and play the reveal sound
        congratsText.enabled = true;
        AudioManager.PlaySFX(revealSound);

        // Wait for move text to appear
        yield return new WaitForSeconds(movesTextWait);

        // Enable the text to display the number of moves it took to solve the puzzle
        movesText.textRoot.gameObject.SetActive(true);
        movesText.text.text = MatrixParent.CurrentMoves.ToString();
        AudioManager.PlaySFX(revealSound);

        // Wait for the high score text to appear
        yield return new WaitForSeconds(fewestMovesTextWait);

        // Enable the text and set it to the minimum on start
        fewestMovesText.textRoot.gameObject.SetActive(true);
        fewestMovesText.text.text = completionDataOnPuzzleStart.FewestMovesString;
        AudioManager.PlaySFX(revealSound);

        // If current level completion is less than it was before, we'll update the text dramatically
        if (GameplayManager.CurrentLevelCompletionData.FewestMoves < completionDataOnPuzzleStart.FewestMoves)
        {
            // Wait for the update to happen
            yield return new WaitForSeconds(fewestMovesTextUpdateWait);

            // Enable the congrats object
            highScoreCongratsObject.SetActive(true);

            // Give the actual fewest moves
            fewestMovesText.text.text = GameplayManager.CurrentLevelCompletionData.FewestMovesString;
            AudioManager.PlaySFX(newHighScoreSound);

            // Punch the scale to make it really flashy
            fewestMovesText.textRoot.DOKill();
            fewestMovesText.textRoot.DOPunchScale(Vector3.one * UISettings.OperatorPunch, UISettings.OperatorPunchTime);
        }
    }
    #endregion
}
