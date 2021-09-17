using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Video;
using TMPro;
using DG.Tweening;
using Hellmade.Sound;

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
    [Tooltip("Time it takes for the tutorial to grow into view")]
    private float growTime = 0.3f;
    [SerializeField]
    [Tooltip("Time it takes for the tutorial to shrink out of view")]
    private float shrinkTime = 0.3f;

    [Space]

    [SerializeField]
    [Tooltip("Sound that plays when the tutorial appears")]
    private AudioClip appearSound;
    [SerializeField]
    [Tooltip("Sound that plays when the tutorial disappears")]
    private AudioClip disappearSound;

    [Space]

    [SerializeField]
    [Tooltip("Event invoked when the tutorial is closed")]
    private UnityEvent onTutorialClosed;
    #endregion

    #region Private Fields
    private static string defaultResourcePath = nameof(TutorialUI);
    #endregion

    #region Public Methods
    public void Open(TutorialData tutorial)
    {
        if (tutorial.OptionalUnlockData.WillUnlock)
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
        // Play the disappearing sound
        EazySoundManager.PlayUISound(disappearSound);

        // Shrink out of view
        rootRect.DOScale(0f, shrinkTime).OnComplete(() =>
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
        rootRect.localScale = Vector3.one * 0.2f;
        rootRect.DOScale(Vector3.one, growTime).SetEase(Ease.InOutBack);

        // Set the display items
        title.text = tutorial.Title;
        image.sprite = tutorial.Sprite;
        videoPlayer.clip = tutorial.Video;
        explanation.text = tutorial.Explanation;

        // Only enable visual elements that have data from the tutorial data
        image.enabled = tutorial.Sprite;
        videoPlayer.enabled = tutorial.Video;
        if (videoPlayer.enabled) videoPlayer.Play();

        // When button is clicked then close the tutorial
        closeButton.onClick.AddListener(Close);

        // Play the appear sound
        EazySoundManager.PlayUISound(appearSound);
    }
    #endregion
}
