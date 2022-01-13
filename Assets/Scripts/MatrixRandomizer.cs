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
    public static int Operations { get; set; } = 5;
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
            GetNeighbors, FunValue, TieBreaker, SearchTerminateTest);
        // Run the search
        finder.Search(matrix, out Matrix result);

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
        Dictionary<Fraction, int> fractionIndex = new Dictionary<Fraction, int>();

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

                // Update fraction counts
                if (fractionIndex.ContainsKey(current))
                {
                    fractionIndex[current]++;
                }
                else fractionIndex[current] = 1;
            }
        }

        // Reduce the score for each subsequent appearance of the same fraction
        foreach(int count in fractionIndex.Values)
        {
            value -= count - 1;
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
    #endregion

    #region Private Methods
    // Don't bother search for more than 100 iterations
    private static bool SearchTerminateTest(Matrix matrix, int iteration)
    {
        return iteration >= Operations;
    }
    // Randomly choose a matrix - used in hill climbing search
    private static Matrix TieBreaker(List<Matrix> matrices)
    {
        return matrices[Random.Range(0, matrices.Count)];
    }
    #endregion
}
