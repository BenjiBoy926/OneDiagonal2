using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AudioLibrary;

public class MusicManager : MonoBehaviour
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Audio clip to play for the music")]
    private AudioClip music;
    #endregion

    #region Public Fields
    public static readonly string prefabPath = nameof(MusicManager);
    #endregion

    #region Monobehaviour Messages
    private void Start()
    {
        AudioManager.PlayMusic(music, true);
    }
    #endregion

    #region Initialize On Load Methods
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void StartMusic()
    {
        GameObject managerPrefab = ResourcesExtensions.InstantiateFromResources(prefabPath, null);
        DontDestroyOnLoad(managerPrefab);
    }
    #endregion
}
