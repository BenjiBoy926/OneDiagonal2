using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEditor.Recorder;
using UnityEditor.Recorder.Input;

using TMPro;

/// <summary>
/// A simple manager used to setup the matrix used for recording
/// operations for tutorials
/// </summary>
public class RecorderLevelManager : MonoBehaviour
{
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
    [Tooltip("Button that can be used to skip recording this level")]
    private Button skipButton;

    [SerializeField]
    [Tooltip("Number of seconds until the countdown completes")]
    private int countdownTime = 3;
    [SerializeField]
    [Tooltip("Number of seconds after solving the puzzle that this recording ends and the next recording begins")]
    private float endingTime = 3;
    #endregion

    #region Private Fields
    private bool skipButtonPressed;
    #endregion

    #region Monobehaviour Messages
    private void Start()
    {
        // Unlock all operations
        PlayerData.UnlockAllOperations();
        // Make recorder verbose to show more information
        RecorderOptions.VerboseMode = true;
        // Record all of the levels
        StartCoroutine(RecordAllLevels());
    }
    #endregion

    #region Private Methods
    private IEnumerator RecordAllLevels()
    {
        LevelData[] levels = LevelSettings.GetAllLevelDataOfType(LevelType.Recorded);

        // Record each level in turn
        foreach(LevelData level in levels)
        {
            yield return RecordOneLevel(level);
        }

        // Show UI to state that 
        startingUI.SetActive(true);
        levelTitle.text = "All done!";
        countdownText.text = "";
    }
    private IEnumerator RecordOneLevel(LevelData level)
    {
        skipButtonPressed = false;

        // Enable the starting UI
        startingUI.SetActive(true);
        levelTitle.text = $"Next Level: '{level.Name}'";

        for(int i = countdownTime; i >= 1; i--)
        {
            countdownText.text = i.ToString();

            // Set time of current count and create a function to detect when one second has passed
            float timeOfCurrentCount = Time.time;
            bool OneSecondElapsed() => Time.time - timeOfCurrentCount >= 1f;

            // Wait until one second has elapsed or the skip button was pressed
            yield return new WaitUntil(() => OneSecondElapsed() || skipButtonPressed);
        }

        if (!skipButtonPressed)
        {
            // Setup the matrix
            startingUI.SetActive(false);
            matrixUI.Setup(level);

            // Start the recording
            RecorderController recorder = GetRecorder(level.Name);
            recorder.PrepareRecording();
            
            if (!recorder.StartRecording())
            {
                Debug.Log("Failed to start the recording!");
            }

            // Wait until the current matrix is identity
            yield return new WaitUntil(() => matrixUI.CurrentMatrix.isIdentity);
            yield return new WaitForSeconds(endingTime);

            // Stop the recording
            recorder.StopRecording();
        }
    }
    private RecorderController GetRecorder(string levelName)
    {
        // Create the settings and setup the controller with them
        RecorderControllerSettings controllerSettings = ScriptableObject.CreateInstance<RecorderControllerSettings>();
        MovieRecorderSettings movieSettings = ScriptableObject.CreateInstance<MovieRecorderSettings>();
        RecorderController controller = new RecorderController(controllerSettings);

        // Set the controller settings
        controllerSettings.SetRecordModeToManual();
        controllerSettings.FrameRatePlayback = FrameRatePlayback.Constant;
        controllerSettings.FrameRate = 30;
        controllerSettings.CapFrameRate = true;
        controllerSettings.AddRecorderSettings(movieSettings);

        // Set the movie settings
        movieSettings.Enabled = true;
        movieSettings.AudioInputSettings.PreserveAudio = true;
        movieSettings.CaptureAlpha = false;
        movieSettings.ImageInputSettings = new GameViewInputSettings();
        movieSettings.OutputFormat = MovieRecorderSettings.VideoRecorderOutputFormat.MP4;
        movieSettings.OutputFile = levelName;
        movieSettings.FileNameGenerator.Root = OutputPath.Root.Project;
        movieSettings.FileNameGenerator.Leaf = "Recordings";
        movieSettings.Take = 1;

        return controller;
    }
    #endregion
}
