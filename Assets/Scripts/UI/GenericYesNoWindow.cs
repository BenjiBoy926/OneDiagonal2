using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class GenericYesNoWindow : MonoBehaviour
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Used to display the yes-no question text")]
    private TextMeshProUGUI messageText;
    [SerializeField]
    private Button yesButton;
    [SerializeField]
    private Button noButton;
    [SerializeField]
    private Button closeButton;
    #endregion

    #region Private Fields
    private static string defaultPrefabName => nameof(GenericYesNoWindow);
    private static string defaultPrefabPath => defaultPrefabName;
    #endregion

    #region Monobehaviour Messages
    private void Start()
    {
        void DestroyCallback() => Destroy(gameObject);
        yesButton.onClick.AddListener(DestroyCallback);
        noButton.onClick.AddListener(DestroyCallback);
        closeButton.onClick.AddListener(DestroyCallback);
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Setup the message and yes-no actions of the generic window
    /// </summary>
    /// <param name="message"></param>
    /// <param name="yesAction"></param>
    /// <param name="noAction"></param>
    public void Setup(string message, UnityAction yesAction, UnityAction noAction, UnityAction closeAction)
    {
        // Setup the text and button callbacks
        messageText.text = message;
        if (yesAction != null) yesButton.onClick.AddListener(yesAction);
        if (noAction != null) noButton.onClick.AddListener(noAction);
        if (closeAction != null) closeButton.onClick.AddListener(closeAction);
    }

    /// <summary>
    /// Create a window by loading a prefab from the resources folder and instantiating it
    /// </summary>
    /// <param name="parent">Parent to instantiate the window under</param>
    /// <param name="prefabPath">The resources path to find the prefab in. 
    /// If null or white space, we use a default path</param>
    /// <returns>The instance of the window that was created</returns>
    public static GenericYesNoWindow InstantiateFromResource(Transform parent, string prefabPath = null)
    {
        if (string.IsNullOrWhiteSpace(prefabPath)) prefabPath = defaultPrefabPath;
        return ResourcesExtensions.InstantiateFromResources<GenericYesNoWindow>(defaultPrefabPath, parent);
    }
    #endregion
}
