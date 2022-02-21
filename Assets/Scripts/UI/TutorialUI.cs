using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Video;
using TMPro;
using DG.Tweening;

public class TutorialUI : MonoBehaviour
{
    #region Public Properties
    public RectTransform RootRect => rootRect;
    public UnityEvent OnTutorialClosed => onTutorialClosed;
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Root rect transform to expand into view when the tutorial appears")]
    private RectTransform rootRect;
    [SerializeField]
    [Tooltip("Text that displays the tutorial title")]
    private TextMeshProUGUI title;
    [SerializeField]
    [Tooltip("Reference to the image that displays the tutorial sprite")]
    private Image image;
    [SerializeField]
    [Tooltip("Reference to the raw image that displays the video texture")]
    private RawImage videoImage;
    [SerializeField]
    [Tooltip("Reference to the player that plays the tutorial video")]
    private VideoPlayer videoPlayer;
    [SerializeField]
    [Tooltip("Reference to the text that displays the explanation")]
    private TextMeshProUGUI explanation;
    [SerializeField]
    [Tooltip("Reference to the button that closes the tutorial")]
    private Button closeButton;

    [Space]

    [SerializeField]
    [Tooltip("Event invoked when the tutorial is closed")]
    private UnityEvent onTutorialClosed;
    #endregion

    #region Private Fields
    private static string defaultResourcePath = nameof(TutorialUI);
    #endregion

    #region Public Methods
    public void Open(TutorialData tutorial, bool displayUpdgrade)
    {
        if (tutorial.OptionalUnlockData.WillUnlock && displayUpdgrade)
        {
            // Disable object while unlock Ui is going
            rootRect.gameObject.SetActive(false);

            // Open the unlock ui first, then open the tutorial
            UnlockUI ui = UnlockUI.InstantiateFromResources(transform.parent);
            ui.Open(tutorial.OptionalUnlockData.TryGetUnlocker());
            ui.OnUnlockFinished.AddListener(() => OpenTutorial(tutorial));
        }
        else OpenTutorial(tutorial);
    }
    public void Close()
    {
        // Shrink out of view
        UISettings.CloseWindow(rootRect).OnComplete(() =>
        {
            onTutorialClosed.Invoke();
            Destroy(gameObject);
        });
    }
    public static TutorialUI InstantiateFromResources(Transform parent, string resourcePath = null)
    {
        if (string.IsNullOrWhiteSpace(resourcePath)) resourcePath = defaultResourcePath;
        return ResourcesExtensions.InstantiateFromResources<TutorialUI>(resourcePath, parent);
    }
    #endregion

    #region Private Methods
    private void OpenTutorial(TutorialData tutorial)
    {
        rootRect.gameObject.SetActive(true);

        // Scale the root rect into view
        UISettings.OpenWindow(rootRect);

        // Set the display items
        title.text = tutorial.Title;
        explanation.text = tutorial.Explanation;
        image.enabled = !tutorial.VideoStreamingPathExists;
        videoImage.enabled = !image.enabled;

        Debug.Log($"Video streaming path exists: {tutorial.VideoStreamingPathExists}" +
            $"\nVideo player url: {tutorial.VideoStreamingURL}");

        // Check if the video streaming path exists
        if (tutorial.VideoStreamingPathExists)
        {
            // Set the video player source to a file url
            videoPlayer.source = VideoSource.Url;
            videoPlayer.url = tutorial.VideoStreamingURL;
            videoPlayer.Play();
        }
        // If video streaming path does not exist then 
        // use the image for the tutorial
        else image.sprite = tutorial.Sprite;

        // When button is clicked then close the tutorial
        closeButton.onClick.AddListener(Close);
    }
    #endregion
}
