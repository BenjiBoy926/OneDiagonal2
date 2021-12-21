using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToLevel : MonoBehaviour
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Level to jump to at the start of this scene")]
    private LevelID level;
    #endregion

    #region Monobehaviour Messages
    private void Awake()
    {
        GameplayManager.PlayLevel(level);
    }
    #endregion
}
