using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TutorialData
{
    #region Public Properties
    public OptionalUnlockOperation OptionalUnlockData => optionalUnlockData;
    public string Title => title;
    public Sprite Sprite => sprite;
    public string VideoStreamingSubPath => videoStreamingSubPath;
    public string VideoStreamingPath => Path.Combine(Application.streamingAssetsPath, VideoStreamingSubPath);
    public string VideoStreamingURL => $"file://{VideoStreamingPath}";
    public bool VideoStreamingPathExists => File.Exists(VideoStreamingPath);
    public string Explanation => explanation;
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Unlock data for this tutorial")]
    private OptionalUnlockOperation optionalUnlockData;
    [SerializeField]
    [Tooltip("Title of the tutorial")]
    private string title;
    [SerializeField]
    [Tooltip("Image to show for the tutorial")]
    private Sprite sprite;
    [SerializeField]
    [Tooltip("Video to show for the tutorial. Input " +
        "as a path in the streaming assets")]
    private string videoStreamingSubPath;
    [SerializeField]
    [TextArea]
    [Tooltip("Text to explain the next step in the tutorial")]
    private string explanation;
    #endregion
}
