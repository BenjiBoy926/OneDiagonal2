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
        // Get a list of all types
        MatrixOperation.Type[] types = (MatrixOperation.Type[])System.Enum.GetValues(typeof(MatrixOperation.Type));

        // Unlock all operations so that recording will work
        foreach(MatrixOperation.Type type in types)
        {
            PlayerData.UnlockOperation(type);
        }

        // Setup the matrix ui
        matrixUI.Setup(LevelSettings.GetLevelData(levelToRecord));
    }
    #endregion
}
