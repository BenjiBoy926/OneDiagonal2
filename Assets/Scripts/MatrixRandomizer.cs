using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrixRandomizer
{
    #region Public Properties
    public static int MinMultiplier
    {
        get => minMultiplier;
        set => minMultiplier = Mathf.Min(value, maxMultiplier);
    }
    public static int MaxMultiplier
    {
        get => maxMultiplier;
        set => maxMultiplier = Mathf.Max(value, minMultiplier);
    }
    public static int Operations
    {
        get => operations;
        set => operations = value;
    }
    #endregion

    #region Private Fields
    private static int operations = 10;
    private static int minMultiplier = -10;
    private static int maxMultiplier = 10;
    #endregion

    #region Public Methods
    public static Matrix Create(int size)
    {
        Matrix matrix = Matrix.Identity(size);

        // Get all the possible row scales and row adds
        Dictionary<int, List<MatrixOperation>> rowScales = GetRowScaleOperations(size);
        Dictionary<int, List<MatrixOperation>> rowAdds = GetRowAddOperations(size);

        // Temporary dictionary used to store row scaling that is not allowed
        Dictionary<int, List<MatrixOperation>> disallowedRowScales = new Dictionary<int, List<MatrixOperation>>();

        for(int i = 0; i < operations; i++)
        {
            // Choose either scaling or adding
            bool chooseScaling = Random.Range(0, 2) == 0 && rowScales.Count > 0;
            List<MatrixOperation> randomList;
            MatrixOperation randomOperation;

            if(chooseScaling)
            {
                // Get all entries as an array
                KeyValuePair<int, List<MatrixOperation>>[] array = rowScales.ToArray();
                // Choose a random list in the array
                randomList = array[Random.Range(0, array.Length)].Value;
            }
            else
            {
                // Get a random list from the row adds
                randomList = rowAdds[Random.Range(0, size)];
            }

            // Choose a random element of the list
            randomOperation = randomList[Random.Range(0, randomList.Count)];
            // Randomly choose the operation or its inverse
            randomOperation = RandomlyChooseThisOrInverse(randomOperation);

            // Operate on the matrix with the random operation
            matrix = matrix.Operate(randomOperation);

            // If this is a row scale, remove row scales from the dictionary to randomly choose from
            if(randomOperation.type == MatrixOperation.Type.Scale)
            {
                List<MatrixOperation> removedList = rowScales[randomOperation.destinationRow];
                rowScales.Remove(randomOperation.destinationRow);
                disallowedRowScales.Add(randomOperation.destinationRow, removedList);
            }
            // We are allowed to apply a scaling to this row again if we just added a different row to it
            else if(disallowedRowScales.ContainsKey(randomOperation.destinationRow))
            {
                rowScales.Add(randomOperation.destinationRow, disallowedRowScales[randomOperation.destinationRow]);
                disallowedRowScales.Remove(randomOperation.destinationRow);
            }
        }

        return matrix;
    }
    #endregion

    #region Private Methods
    // Return a dictionary that maps the destination row 
    // with the list of row scale operations that operate on that row
    private static Dictionary<int, List<MatrixOperation>> GetRowScaleOperations(int rows)
    {
        Dictionary<int, List<MatrixOperation>> operations = new Dictionary<int, List<MatrixOperation>>();

        // Iterate over all rows
        for(int i = 0; i < rows; i++)
        {
            // Add a new list at this key in the dictionary
            operations.Add(i, new List<MatrixOperation>());

            // Create a row scale operation for every scalar on every row
            for(int scalar = minMultiplier; scalar <= maxMultiplier; scalar++)
            {
                if(scalar <= -1 || scalar >= 2)
                {
                    operations[i].Add(MatrixOperation.RowScale(i, new Fraction(scalar, 1)));
                }
            }
        }

        return operations;
    }
    // Return a dictionary that maps the destination row to a list of row add operations
    private static Dictionary<int, List<MatrixOperation>> GetRowAddOperations(int rows)
    {
        Dictionary<int, List<MatrixOperation>> operations = new Dictionary<int, List<MatrixOperation>>();

        // Loop through all possible destination rows
        for (int destination = 0; destination < rows; destination++)
        {
            // Add a new list for this key
            operations.Add(destination, new List<MatrixOperation>());

            // Loop through all possible source rows
            for (int source = 0; source < rows; source++)
            {
                // If the source is not the destination then add a matrix operation
                if (source != destination)
                {
                    operations[destination].Add(MatrixOperation.RowAdd(source, destination, Fraction.one));
                }
            }
        }

        return operations;
    }
    private static MatrixOperation RandomlyChooseThisOrInverse(MatrixOperation matrixOperation)
    {
        return Random.Range(0, 2) == 0 ? matrixOperation : matrixOperation.Inverse;
    }
    #endregion
}
