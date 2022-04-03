using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EffectsManager : MonoBehaviour
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Array of flash effect config to instantiate for each pool")]
    private ArrayOnEnum<EffectType, EffectConfig> configs = new ArrayOnEnum<EffectType, EffectConfig> ();
    #endregion

    #region Private Fields
    private static EffectsManager instance;
    private ArrayOnEnum<EffectType, EffectPool> pools = new ArrayOnEnum<EffectType, EffectPool>();
    #endregion

    #region Initialize Methods
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Initialize()
    {
        // Instantiate the effects manager from the resources and make it not destroy on load
        instance = ResourcesExtensions.InstantiateFromResources<EffectsManager>(nameof(EffectsManager), null);
        DontDestroyOnLoad(instance);

        // Re-initialize the pools on scene loaded
        SceneManager.sceneLoaded += instance.OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Start();
    }
    #endregion

    #region Monobehaviour Messages
    private void Start()
    {
        // Get all effect types
        EffectType[] types = (EffectType[])System.Enum.GetValues(typeof(EffectType));
    
        // Create a new effect pool for every type
        foreach (EffectType type in types)
        {
            pools.Set(type, new EffectPool(configs.Get(type)));
        }
    }
    #endregion

    #region Public Methods
    public static void Flash(Transform transform, EffectType type, Color color)
    {
        FlashEffect flash = instance.pools.Get(type).FlashPool.Get();
        flash.transform.SetParent(transform, false);
        flash.UpdateUI();
        flash.Flash(color);
    }
    #endregion
}
