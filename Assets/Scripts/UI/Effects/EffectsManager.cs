using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EffectsManager : MonoBehaviour
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Array of flash effect config to instantiate for each pool")]
    private ArrayOnEnum<EffectType, OutlineEffect> prefabs = new ArrayOnEnum<EffectType, OutlineEffect> ();
    [SerializeField]
    [Tooltip("Initial amount of effects to create for each type")]
    private int initialSize = 2;
    #endregion

    #region Private Fields
    private static EffectsManager instance;
    private ArrayOnEnum<EffectType, Pool<OutlineEffect>> pools = new ArrayOnEnum<EffectType, Pool<OutlineEffect>>();
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
            // Get the config for this type of effect
            OutlineEffect prefab = prefabs.Get(type);

            // Set the pool in the array to a new pool
            pools.Set(type, new Pool<OutlineEffect>(
                initialSize,
                () => Instantiate(prefab),
                effect => effect.Image.color.a <= 0.5f));
        }
    }
    #endregion

    #region Public Methods
    public static OutlineEffect FadeOutOutline(Transform transform, EffectType type, Color color)
    {
        OutlineEffect outline = instance.pools.Get(type).Get();
        outline.transform.SetParent(transform, false);
        outline.UpdateUI();
        outline.FadeOut(color);
        return outline;
    }
    public static OutlineEffect FadeInOutline(Transform transform, EffectType type, Color color)
    {
        OutlineEffect outline = instance.pools.Get(type).Get();
        outline.transform.SetParent(transform, false);
        outline.UpdateUI();
        outline.FadeIn(color);
        return outline;
    }
    #endregion
}
