using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[System.Serializable]
public class TutorialData
{
    #region Public Properties
    public OptionalUnlockOperation OptionalUnlockData => optionalUnlockData;
    public string Title => title;
    public Sprite Sprite => sprite;
    public VideoClip Video => video;
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
    [Tooltip("Video to show for the tutorial")]
    private VideoClip video;
    [SerializeField]
    [TextArea]
    [Tooltip("Text to explain the next step in the tutorial")]
    private string explanation;
    #endregion
}
