using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AudioPanel : MonoBehaviour
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Transform of the window with the audio controls")]
    private RectTransform window;
    [SerializeField]
    [Tooltip("Button that closes the audio window")]
    private Button closeButton;
    #endregion

    #region Private Fields
    private static AudioPanel instance;
    #endregion

    #region Initialize Methods
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Initialize()
    {
        instance = ResourcesExtensions.InstantiateFromResources<AudioPanel>(nameof(AudioPanel), null);
        DontDestroyOnLoad(instance);
    }
    #endregion

    #region Public Methods
    public static void Open()
    {
        instance.gameObject.SetActive(true);
        UISettings.OpenWindow(instance.window);
    }
    #endregion

    #region Monobehaviour Messages
    private void Start()
    {
        closeButton.onClick.AddListener(Close);
        Disable();
    }
    #endregion

    #region Private Methods
    private void Close()
    {
        UISettings.CloseWindow(window).OnComplete(Disable);
    }
    private void Disable()
    {
        gameObject.SetActive(false);
    }
    #endregion
}
