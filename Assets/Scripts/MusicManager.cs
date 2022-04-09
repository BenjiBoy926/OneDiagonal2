using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AudioUtility;

public class MusicManager : MonoBehaviour
{
    #region Public Properties
    public static AudioSource MusicSource => instance.musicSource;
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Audio clip to play for the music")]
    private AudioClip music;
    #endregion

    #region Public Fields
    public static readonly string prefabPath = nameof(MusicManager);
    #endregion

    #region Private Fields
    private static MusicManager instance;
    private AudioSource musicSource;
    #endregion

    #region Monobehaviour Messages
    private void Awake()
    {
        musicSource = AudioManager.PlayMusic(music, looping: true);
    }
    #endregion

    #region Initialize On Load Methods
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void StartMusic()
    {
        MusicManager managerInstance = ResourcesExtensions.InstantiateFromResources<MusicManager>(prefabPath, null);

        instance = managerInstance;
        DontDestroyOnLoad(managerInstance);
    }
    #endregion
}
