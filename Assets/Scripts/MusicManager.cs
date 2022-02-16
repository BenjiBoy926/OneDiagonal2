using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hellmade.Sound;

public class MusicManager
{
    #region Public Fields
    public static readonly string prefabPath = nameof(MusicManager);
    #endregion

    #region Initialize On Load Methods
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void StartMusic()
    {
        GameObject managerPrefab = ResourcesExtensions.InstantiateFromResources(prefabPath, null);
        Object.DontDestroyOnLoad(managerPrefab);
    }
    #endregion
}
