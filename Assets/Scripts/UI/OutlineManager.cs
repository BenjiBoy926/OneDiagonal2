using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OutlineManager : MonoBehaviour
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Array of flash effect config to instantiate for each pool")]
    private ArrayOnEnum<OutlineType, OutlineEffect> prefabs = new ArrayOnEnum<OutlineType, OutlineEffect> ();
    [SerializeField]
    [Tooltip("Initial amount of effects to create for each type")]
    private int initialSize = 2;
    #endregion

    #region Private Fields
    private static OutlineManager instance;
    private ArrayOnEnum<OutlineType, Pool<OutlineEffect>> pools = new ArrayOnEnum<OutlineType, Pool<OutlineEffect>>();
    #endregion

    #region Initialize Methods
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Initialize()
    {
        // Instantiate the effects manager from the resources and make it not destroy on load
        instance = ResourcesExtensions.InstantiateFromResources<OutlineManager>(nameof(OutlineManager), null);
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
        OutlineType[] types = (OutlineType[])System.Enum.GetValues(typeof(OutlineType));
    
        // Create a new effect pool for every type
        foreach (OutlineType type in types)
        {
            // Set the pool in the array to a new pool
            pools.Set(type, new Pool<OutlineEffect>(
                initialSize,
                () => InstantiateOutline(type),
                OutlineIsUsable,
                MakeOutlineUsable));
        }
    }
    #endregion

    #region Public Methods
    public static OutlineEffect FadeOutOutline(Transform transform, OutlineType type, Color color)
    {
        OutlineEffect outline = instance.pools.Get(type).Get();
        outline.transform.SetParent(transform, false);
        outline.UpdateUI();
        outline.FadeOut(color);
        return outline;
    }
    public static OutlineEffect FadeInOutline(Transform transform, OutlineType type, Color color)
    {
        OutlineEffect outline = instance.pools.Get(type).Get();
        outline.transform.SetParent(transform, false);
        outline.UpdateUI();
        outline.FadeIn(color);
        return outline;
    }
    #endregion

    #region Private Methods
    private OutlineEffect InstantiateOutline(OutlineType type)
    {
        return Instantiate(prefabs.Get(type));
    }
    private bool OutlineIsUsable(OutlineEffect effect)
    {
        return effect.IsRemoved;
    }
    private void MakeOutlineUsable(OutlineEffect effect)
    {
        effect.Remove();
    }
    #endregion
}
