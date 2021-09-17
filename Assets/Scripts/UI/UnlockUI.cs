using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;
using TMPro;
using Hellmade.Sound;

public class UnlockUI : MonoBehaviour
{
    #region Public Properties
    public UnityEvent OnUnlockFinished => onUnlockFinished;
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Reference to the text to display the congratulations")]
    private TextMeshProUGUI congrats;
    [SerializeField]
    [Tooltip("Image that displays the unlock sprite")]
    private Image unlockImage;
    [SerializeField]
    [Tooltip("Rect transform to spin around dramatically while displaying the UI")]
    private RectTransform spinningTransform;
    [SerializeField]
    [Tooltip("Root rect transform of the UI to scale in and out")]
    private RectTransform rootTransform;
    [SerializeField]
    [Tooltip("Button that used to close the ui")]
    private Button closeButton;

    [Space]

    [SerializeField]
    [Tooltip("Time it takes for the unlock ui to grow to full size")]
    private float growTime = 3f;
    [SerializeField]
    [Tooltip("Time it takes for the ui to shrink out of view")]
    private float shrinkTime = 0.3f;
    [SerializeField]
    [Tooltip("Amount that the spinning transform rotates each second")]
    private float rotationSpeed = 30f;

    [Space]

    [SerializeField]
    [Tooltip("Sound that plays when the ui is displayed")]
    private AudioClip displaySound;

    [Space]

    [SerializeField]
    [Tooltip("Event invoked when the unlock ui finishes")]
    private UnityEvent onUnlockFinished;
    #endregion

    #region Private Fields
    private static string defaultResourcePath = nameof(UnlockUI);
    #endregion

    #region Public Methods
    public void Open(UnlockOperation unlock)
    {
        rootTransform.gameObject.SetActive(true);

        // Start tiny then scale
        rootTransform.localScale = Vector3.one * 0.2f;
        rootTransform.DOScale(1f, growTime).SetEase(Ease.InOutBack);

        // Have the spinning transform rotate infinitely
        spinningTransform.DORotate(Vector3.forward * rotationSpeed, 1f).SetLoops(-1).SetEase(Ease.Linear);

        // Setup the text and image
        congrats.text = "Congratulations!\nYou've unlocked the operation: Row " + unlock.OperationType.ToString();
        unlockImage.sprite = unlock.UnlockSprite;

        // Close the ui when the button is clicked
        closeButton.onClick.AddListener(Close);

        // Play the sound when we are displayed
        EazySoundManager.PlayUISound(displaySound);
    }
    public void Close()
    {
        rootTransform.DOKill();
        rootTransform.DOScale(0f, shrinkTime).OnComplete(() => 
        {
            onUnlockFinished.Invoke();
            Destroy(gameObject);
        });
    }
    public static UnlockUI InstantiateFromResources(Transform parent, string prefabPath = null)
    {
        if (string.IsNullOrWhiteSpace(prefabPath)) prefabPath = defaultResourcePath;
        return ResourcesExtensions.InstantiateFromResources<UnlockUI>(prefabPath, parent);
    }
    #endregion
}
