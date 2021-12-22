using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A simple manager used to setup the matrix used for recording
/// operations for tutorials
/// </summary>
public class RecorderLevelManager : MonoBehaviour
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Level to use for recording")]
    private LevelID levelToRecord;
    [SerializeField]
    [Tooltip("Matrix to setup for recording")]
    private MatrixUI matrixUI;
    #endregion

    #region Monobehaviour Messages
    private void Start()
    {
        // Unlock all operations
        PlayerData.UnlockAllOperations();
        // Setup the matrix ui
        matrixUI.Setup(LevelSettings.GetLevelData(levelToRecord));
    }
    #endregion
}
