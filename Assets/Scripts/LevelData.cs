using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Data", menuName = "Level Data")]
public class LevelData : ScriptableObject
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Starting value for the matrix, if it is an exact value")]
    private Matrix startingValue;
    #endregion

    public Matrix GetStartingMatrix()
    {
        return startingValue;
        // TODO: if the matrix is random for this level then we need to create a random matrix
    }
}
