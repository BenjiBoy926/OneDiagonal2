using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelTutorialData
{
    #region Public Properties
    public LevelID Level => level;
    public TutorialData[] Tutorials => tutorials;
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Level that these tutorials are displayed in")]
    private LevelID level;
    [SerializeField]
    [Tooltip("List of tutorials in the order they are shown")]
    private TutorialData[] tutorials;
    #endregion
}
