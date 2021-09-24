using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextLevelButton : MonoBehaviour
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("The button that sends the player to the next level")]
    private Button button;
    [SerializeField]
    [Tooltip("Canvas group that controls visibility for this button")]
    private CanvasGroup group;
    #endregion

    #region Monobehaviour Messages
    private void Start()
    {
        if(LevelSettings.IsLastLevel(GameplayManager.CurrentLevelID))
        {
            group.alpha = 0.3f;
            group.blocksRaycasts = false;
        }
        else
        {
            group.alpha = 1f;
            group.blocksRaycasts = true;
            button.onClick.AddListener(GameplayManager.PlayNextLevel);
        }
    }
    #endregion
}
