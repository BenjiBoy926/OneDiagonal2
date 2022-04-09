using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// A dummy class used by the recorder level manager
/// to schedule coroutines in the scene
/// </summary>
public class RecorderLevelEntryPoint : MonoBehaviour
{
    #region Public Properties
    public MatrixUI MatrixUI => matrixUI;
    public GameObject StartingUI => startingUI;
    public TextMeshProUGUI LevelTitle => levelTitle;
    public TextMeshProUGUI CountdownText => countdownText;
    public int CountdownTime => countdownTime;
    public float EndingTime => endingTime;
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Matrix to setup for recording")]
    private MatrixUI matrixUI;

    [SerializeField]
    [Tooltip("Parent of the UI objects that display at the start of the recording")]
    private GameObject startingUI;
    [SerializeField]
    [Tooltip("Text used to display the level that is about to be played")]
    private TextMeshProUGUI levelTitle;
    [SerializeField]
    [Tooltip("Text used to display the countdown until the level starts recording")]
    private TextMeshProUGUI countdownText;

    [SerializeField]
    [Tooltip("Number of seconds until the countdown completes")]
    private int countdownTime = 3;
    [SerializeField]
    [Tooltip("Number of seconds after solving the puzzle that this recording ends and the next recording begins")]
    private float endingTime = 3;
    #endregion
}
