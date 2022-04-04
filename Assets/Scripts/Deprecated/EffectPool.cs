using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPool
{
    #region Public Properties
    public Pool<FlashEffect> FlashPool => flashPool;
    public Pool<OutlineEffect> OutlinePool => outlinePool;
    #endregion

    #region Private Fields
    private Pool<FlashEffect> flashPool;
    private Pool<OutlineEffect> outlinePool;
    #endregion

    #region Constructors
    public EffectPool(EffectConfig config)
    {
        //flashPool = new Pool<FlashEffect>(
        //    config.InitialSize,
        //    () => Object.Instantiate(config.FlashEffectPrefab),
        //    effect => effect.Image.color.a <= 0.01f);

        outlinePool = new Pool<OutlineEffect>(
            config.InitialSize,
            () => Object.Instantiate(config.OutlineEffectPrefab),
            effect => effect.Image.color.a <= 0.01f);
    }
    #endregion
}
