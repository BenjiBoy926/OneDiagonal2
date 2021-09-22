using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    #region Public Typedefs
    public enum Type
    {
        Fixed, Random
    }
    #endregion

    #region Public Properties
    public TutorialData[] Tutorials => tutorials;
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Size of the matrix to solve")]
    private int size = 3;
    [SerializeField]
    [Tooltip("Will the level have a fixed initial state or a random initial state")]
    private Type type;
    [SerializeField]
    [Tooltip("List of operations the player is expected to take to solve the puzzle")]
    private List<MatrixOperation> intendedSolution;
    [SerializeField]
    [Tooltip("List of tutorials that introduce this level the first time that it is played")]
    private TutorialData[] tutorials;
    #endregion

    #region Public Methods
    public Matrix GetStartingMatrix()
    {
        Matrix startingMatrix = Matrix.Identity(size);

        if(type == Type.Fixed)
        {
            for (int i = intendedSolution.Count - 1; i >= 0; i--)
            {
                startingMatrix = startingMatrix.Operate(intendedSolution[i].Inverse);
            }
        }
        else
        {
            // Do many, MANY things
        }
        
        return startingMatrix;
    }
    #endregion
}
