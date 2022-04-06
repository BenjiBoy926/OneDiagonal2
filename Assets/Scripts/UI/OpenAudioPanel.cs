using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenAudioPanel : MonoBehaviour
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Reference to the button that opens the audio panel when clicked")]
    private Button button;
    #endregion

    #region Monobehaviour Messages
    private void Start()
    {
        button.onClick.AddListener(AudioPanel.Open);
    }
    #endregion
}
