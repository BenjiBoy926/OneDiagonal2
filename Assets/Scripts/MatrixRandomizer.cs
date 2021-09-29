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

        return matrix;
    }
    #endregion

    #region Private Methods
    private static MatrixOperation[] GetRowScaleOperations(int rows)
    {
        MatrixOperation[] rowScales = new MatrixOperation[Mathf.Abs(maxMultiplier - minMultiplier) + 1];

        // Iterate over all rows
        for(int i = 0; i < rows; i++)
        {
            // Create a row scale operation for every scalar on every row
            for(int scalar = minMultiplier; scalar <= maxMultiplier; scalar++)
            {
                int index = Mathf.Abs(minMultiplier - scalar);
                rowScales[index] = MatrixOperation.RowScale(i, new Fraction(scalar, 1));
            }
        }

        // Choose only scalars below or equal to -1, and above 1
        rowScales = rowScales.Where(x => x.scalar <= -Fraction.one && x.scalar > Fraction.one).ToArray();
        return rowScales;
    }
    private static MatrixOperation[] GetRowAddOperations(int rows)
    {
        MatrixOperation[] rowAdds = new MatrixOperation[rows * (rows - 1)];

        // Loop through all possible source rows
        for(int source = 0; source < rows; source++)
        {
            // Loop through all possible destination rows
            for(int destination = 0; destination < rows; destination++)
            {
                // If the source is not the destination then add a matrix operation
                if(source != destination)
                {
                    int col = destination > source ? destination - 1 : destination;
                    int index = MyMath.Index2Dto1D(source, col, rows - 1);
                    rowAdds[index] = MatrixOperation.RowAdd(source, destination, Fraction.one);
                }
            }
        }

        return rowAdds;
    }
    private static MatrixOperation RandomlyChooseThisOrInverse(MatrixOperation matrixOperation)
    {
        return Random.Range(0, 2) == 0 ? matrixOperation : matrixOperation.Inverse;
    }
    #endregion
}
