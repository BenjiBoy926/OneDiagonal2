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
    public string Name => name;
    public string EditorDisplayName => name + (tutorials.Length > 0 ? "*" : "");
    public int Size => size;
    public TutorialData[] Tutorials => tutorials;
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("String title for this level")]
    private string name = "Level";
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
            // Perform the inverse of each operation in the reverse order
            for (int i = intendedSolution.Count - 1; i >= 0; i--)
            {
                startingMatrix = startingMatrix.Operate(intendedSolution[i].Inverse);
            }
        }
        else
        {
            // Create a random matrix with the given size
            MatrixRandomizer.Operations = (size * 2) - 1;
            startingMatrix = MatrixRandomizer.Create(size);
        }
        
        return startingMatrix;
    }
    #endregion
}
