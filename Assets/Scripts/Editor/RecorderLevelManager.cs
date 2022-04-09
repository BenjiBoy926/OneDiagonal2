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
public class RecorderLevelManager
{
    #region Initialize Methods
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Start()
    {
        RecorderLevelEntryPoint scheduler = Object.FindObjectOfType<RecorderLevelEntryPoint>();

        if (scheduler)
        {
            // Unlock all operations
            PlayerData.UnlockAllOperations();
            // Make recorder verbose to show more information
            RecorderOptions.VerboseMode = true;

            // Do something to stop the music
            MusicManager.MusicSource.Stop();

            // Record all of the levels
            scheduler.StartCoroutine(RecordAllLevels(scheduler));
        }
    }
    #endregion

    #region Private Methods
    private static IEnumerator RecordAllLevels(RecorderLevelEntryPoint entry)
    {
        LevelData[] levels = LevelSettings.GetAllLevelDataOfType(LevelType.Recorded);

        // Record each level in turn
        foreach(LevelData level in levels)
        {
            yield return RecordOneLevel(level, entry);
        }

        // Show UI to state that 
        entry.StartingUI.SetActive(true);
        entry.LevelTitle.text = "All done!";
        entry.CountdownText.text = "";
    }
    private static IEnumerator RecordOneLevel(LevelData level, RecorderLevelEntryPoint entry)
    {
        // Enable the starting UI
        entry.StartingUI.SetActive(true);
        entry.LevelTitle.text = $"Next Level: '{level.Name}'";

        for(int i = entry.CountdownTime; i >= 1; i--)
        {
            entry.CountdownText.text = i.ToString();

            // Set time of current count and create a function to detect when one second has passed
            yield return new WaitForSeconds(1f);
        }

        // Setup the matrix
        entry.StartingUI.SetActive(false);
        entry.MatrixUI.Setup(level);

        // Start the recording
        RecorderController recorder = GetRecorder(level.Name);
        recorder.PrepareRecording();
            
        if (!recorder.StartRecording())
        {
            Debug.Log("Failed to start the recording!");
        }

        // Wait until the current matrix is identity
        yield return new WaitUntil(() => entry.MatrixUI.CurrentMatrix.isIdentity);
        yield return new WaitForSeconds(entry.EndingTime);

        // Stop the recording
        recorder.StopRecording();
    }
    private static RecorderController GetRecorder(string levelName)
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
