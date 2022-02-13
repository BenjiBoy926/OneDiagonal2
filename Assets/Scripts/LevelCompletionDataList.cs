using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// So that the array on enum is property serialized
[System.Serializable]
public class LevelCompletionDataList
{
    #region Public Properties
    public LevelCompletionData[] Array => array;
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("List of completion datas")]
    private LevelCompletionData[] array;
    [SerializeField]
    [HideInInspector]
    // Used by the custom property drawer
    // to make the array parallel to all the levels
    // of a specific type
    private string levelTypeFilter;
    #endregion

    #region Constructors
    public LevelCompletionDataList(int size)
    {
        array = new LevelCompletionData[size];
    }
    #endregion
}