using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Data", menuName = "Level Data")]
public class LevelData : ScriptableObject
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Size of the matrix to solve")]
    private int size = 3;
    [SerializeField]
    [Tooltip("List of operations the player is expected to take to solve the puzzle")]
    private List<MatrixOperation> intendedSolution;
    #endregion

    public Matrix GetStartingMatrix()
    {
        Matrix startingMatrix = Matrix.Identity(size);
        for(int i = intendedSolution.Count - 1; i >= 0; i--)
        {
            startingMatrix = startingMatrix.Operate(intendedSolution[i].Inverse);
        }
        return startingMatrix;
    }
}
