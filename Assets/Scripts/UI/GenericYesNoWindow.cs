using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class GenericYesNoWindow : MonoBehaviour
{
    #region Public Typedefs
    public enum ResponseType { Yes, No, Cancel }
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Rect transform for the window that shrinks in and out of view")]
    private RectTransform window;

    [Space]

    [SerializeField]
    [Tooltip("Used to display the yes-no question text")]
    private TextMeshProUGUI messageText;
    [SerializeField]
    [Tooltip("List of buttons for each action on the yes-no window")]
    private ArrayOnEnum<ResponseType, Button> responseButtons;
    #endregion

    #region Private Fields
    private static string defaultPrefabName => nameof(GenericYesNoWindow);
    private static string defaultPrefabPath => defaultPrefabName;
    // List of functions to invoke for each window action
    private ArrayOnEnum<ResponseType, UnityAction> responses = new ArrayOnEnum<ResponseType, UnityAction>();
    #endregion

    #region Monobehaviour Messages
    private void Start()
    {
        // Local function returns a functor with the parameter 
        // so that the loop control variable is not captured in a lambda
        UnityAction CloseWindowFunctor(ResponseType response) => () => Close(response);

        // Make each button close the window with the corresponding action
        for(int i = 0; i < responseButtons.Data.Length; i++)
        {
            responseButtons.Data[i].onClick.AddListener(CloseWindowFunctor((ResponseType)i));
        }
    }
    #endregion

    #region Public Methods
    public void Open(string message)
    {
        // Setup the text and button callbacks
        messageText.text = message;
        UISettings.OpenWindow(window);
    }
    public void Close(ResponseType action)
    {
        UISettings.CloseWindow(window).OnComplete(() =>
        {
            // Try to get and invoke the requested response
            UnityAction response = responses.Get(action);
            if (response != null) response.Invoke();

            // Destroy the window
            Destroy(gameObject);
        });
    }
    public void SetResponse(ResponseType action, UnityAction response) => responses.Set(action, response);
    public static GenericYesNoWindow InstantiateFromResource(Transform parent, string prefabPath = null)
    {
        if (string.IsNullOrWhiteSpace(prefabPath)) prefabPath = defaultPrefabPath;
        return ResourcesExtensions.InstantiateFromResources<GenericYesNoWindow>(defaultPrefabPath, parent);
    }
    #endregion
}
