using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    #region Public Typedefs
    [System.Serializable]
    public class LevelSelectorButtonData
    {
        [Tooltip("Layout group for each button copy")]
        public LayoutGroup parent;
        [Tooltip("Prefab to copy for each button")]
        public LevelSelectorButton prefab;
    }
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Reference to the main button to copy")]
    private ArrayOnEnum<LevelType, LevelSelectorButtonData> buttonData;
    #endregion

    #region Private Fields
    private static LevelType levelType = LevelType.Enumerated;
    #endregion

    #region Monobehaviour Messages
    private void Start()
    {
        // Get a list of all the ids with the given type
        LevelID[] ids = LevelSettings.GetAllLevelIDsOfType(levelType);
        // Cache the level selector button data
        LevelSelectorButtonData data = buttonData.Get(levelType);

        // Create a selector button for each ID and set it up
        foreach(LevelID id in ids)
        {
            LevelSelectorButton instance = Instantiate(data.prefab, data.parent.transform);
            instance.Setup(id);
        }
    }
    #endregion

    #region Public Methods
    public static void SelectLevelsWithType(LevelType levelType)
    {
        LevelSelector.levelType = levelType;
        SceneManager.LoadScene("LevelSelector");
    }
    #endregion
}
