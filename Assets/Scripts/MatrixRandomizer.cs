using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrixRandomizer
{
    #region Public Properties
    public static int MaxAllowableNumber { get; set; } = 12;
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
    public static int Operations { get; set; } = 10;
    #endregion

    #region Private Fields
    private static int minMultiplier = -5;
    private static int maxMultiplier = 5;
    #endregion

    #region Public Methods
    public static Matrix Create(int size)
    {
        // Create the identity matrix
        Matrix matrix = Matrix.Identity(size);
        // Create an object to help find a fun matrix to solve
        HillClimbingLocalSearch<Matrix> finder = new HillClimbingLocalSearch<Matrix>(
            GetNeighbors, FunValue, TieBreaker, MatrixIsFun, SearchTerminateTest);
        // Matrix that results from the search
        Matrix result;

        // Run the search. If it couldn't meet the goal test then log a warning
        if(!finder.Search(matrix, out result))
        {
            Debug.LogWarning($"{nameof(MatrixRandomizer)}: failed to find a matrix with the max fun value after 100 interations");
        }

        return result;
    }
    /// <summary>
    /// Compute the "fun" value for a matrix
    /// Matrices with negative fun value are considered not interesting to solve,
    /// whereas matrices with a value of 0 should be fun to solve!
    /// </summary>
    /// <param name="matrix"></param>
    /// <returns></returns>
    public static int FunValue(Matrix matrix)
    {
        int value = 0;

        for(int i = 0; i < matrix.rows; i++)
        {
            for(int j = 0; j < matrix.cols; j++)
            {
                Fraction current = matrix.Get(i, j);

                // Dock a point for being a one
                if(current == Fraction.one)
                {
                    value--;

                    // Dock another point for being a one in the correct position
                    if (i == j) value--;
                }

                // Dock a point for being a zero
                if(current == Fraction.zero)
                {
                    value--;

                    // Dock another point for being a zero in the correct position
                    if (i != j) value--;
                }

                // Subtract more points the further the numerator/denominator are from max allowable number
                if(Mathf.Abs(current.numerator) > MaxAllowableNumber)
                {
                    value -= Mathf.Abs(current.numerator) - MaxAllowableNumber;
                }
                if (current.denominator > MaxAllowableNumber) value -= current.denominator - MaxAllowableNumber;
            }
        }

        return value;
    }
    /// <summary>
    /// Get the "neighbors" of this matrix - that is,
    /// all matrices that are only one row operation away
    /// from this matrix
    /// </summary>
    /// <param name="matrix"></param>
    /// <returns></returns>
    public static List<Matrix> GetNeighbors(Matrix matrix)
    {
        // List of neighbors to return
        List<Matrix> neighbors = new List<Matrix>();
        // List of operations to perform to get each neighbor
        List<MatrixOperation> operations = new List<MatrixOperation>();

        for (int destinationRow = 0; destinationRow < matrix.rows; destinationRow++)
        {
            // Go through all possible multipliers
            for (int multiplier = minMultiplier; multiplier <= maxMultiplier; multiplier++)
            {
                // Check to make sure the multiplier is not essentially a no-op
                if (multiplier < -1 || multiplier > 1)
                {
                    // Add multiply and divide by this number
                    operations.Add(MatrixOperation.RowScale(destinationRow, new Fraction(multiplier)));
                    operations.Add(MatrixOperation.RowScale(destinationRow, new Fraction(1, multiplier)));
                }
            }

            // Go through all possible source rows
            for (int sourceRow = 0; sourceRow < matrix.rows; sourceRow++)
            {
                if(sourceRow != destinationRow)
                {
                    // Add operations for row add and subtract
                    operations.Add(MatrixOperation.RowAdd(sourceRow, destinationRow, Fraction.one));
                    operations.Add(MatrixOperation.RowAdd(sourceRow, destinationRow, -Fraction.one));
                    // Add row swap operation
                    operations.Add(MatrixOperation.RowSwap(destinationRow, sourceRow));
                }
            }
        }

        // Add a neighbor for each operation
        foreach(MatrixOperation operation in operations)
        {
            neighbors.Add(matrix.Operate(operation));
        }

        return neighbors;
    }
    /// <summary>
    /// True if the given matrix is "fun", 
    /// meaning has the max possible fun value (currently 0)
    /// </summary>
    /// <param name="matrix"></param>
    /// <returns></returns>
    public static bool MatrixIsFun(Matrix matrix)
    {
        return FunValue(matrix) >= 0;
    }
    #endregion

    #region Private Methods
    // Don't bother search for more than 100 iterations
    private static bool SearchTerminateTest(Matrix matrix, int iteration)
    {
        return iteration >= 100;
    }
    // Randomly choose a matrix - used in hill climbing search
    private static Matrix TieBreaker(List<Matrix> matrices)
    {
        return matrices[Random.Range(0, matrices.Count)];
    }
    // Try to create a matrix with a certain methodology
    private static Matrix BruteForceMatrixCreate(int size)
    {
        Matrix matrix = Matrix.Identity(size);

        // Get all the possible row scales and row adds
        Dictionary<int, List<MatrixOperation>> rowScales = GetRowScaleOperations(size);
        Dictionary<int, List<MatrixOperation>> rowAdds = GetRowAddOperations(size);

        // Temporary dictionary used to store row scaling that is not allowed
        Dictionary<int, List<MatrixOperation>> disallowedRowScales = new Dictionary<int, List<MatrixOperation>>();

        for (int i = 0; i < Operations; i++)
        {
            // Choose either scaling or adding
            bool chooseScaling = Random.Range(0, 2) == 0 && rowScales.Count > 0;
            List<MatrixOperation> randomList;
            MatrixOperation randomOperation;

            if (chooseScaling)
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
            if (randomOperation.type == MatrixOperation.Type.Scale)
            {
                List<MatrixOperation> removedList = rowScales[randomOperation.destinationRow];
                rowScales.Remove(randomOperation.destinationRow);
                disallowedRowScales.Add(randomOperation.destinationRow, removedList);
            }
            // We are allowed to apply a scaling to this row again if we just added a different row to it
            else if (disallowedRowScales.ContainsKey(randomOperation.destinationRow))
            {
                rowScales.Add(randomOperation.destinationRow, disallowedRowScales[randomOperation.destinationRow]);
                disallowedRowScales.Remove(randomOperation.destinationRow);
            }
        }

        return matrix;
    }
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
                if(scalar < -1 || scalar > 1)
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
