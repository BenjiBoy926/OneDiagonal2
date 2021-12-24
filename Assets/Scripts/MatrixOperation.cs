
[System.Serializable]
public struct MatrixOperation
{
    #region Typedefs
    public enum Type { Swap, Scale, Add }
    #endregion

    #region Public Properties
    public static MatrixOperation Invalid => RowSwap(-1, -1);
    public MatrixOperation Inverse => type switch
    {
        Type.Scale => RowScale(destinationRow, scalar.reciprocal),
        Type.Add => RowAdd(sourceRow, destinationRow, -scalar),
        _ => this
    };
    public bool IsValid => type switch
    {
        Type.Swap => destinationRow >= 0,
        Type.Scale => destinationRow >= 0 && sourceRow >= 0,
        Type.Add => destinationRow >= 0,
        _ => true
    };
    public string OperationString => type switch
    {
        Type.Swap => "->",
        Type.Scale => "*",
        Type.Add => "+",
        _ => "nop"
    };
    #endregion

    #region Public Fields
    public Type type;
    public int destinationRow;
    public int sourceRow;
    public Fraction scalar;
    #endregion

    #region Constructors
    private MatrixOperation(Type type, int destinationRow, int sourceRow, Fraction scalar)
    {
        this.type = type;
        this.destinationRow = destinationRow;
        this.sourceRow = sourceRow;
        this.scalar = scalar;
    }
    #endregion

    #region Factory Methods
    public static MatrixOperation RowSwap(int destinationRow, int sourceRow)
    {
        return new MatrixOperation(Type.Swap, destinationRow, sourceRow, Fraction.one);
    }
    public static MatrixOperation RowScale(int destinationRow, Fraction scalar)
    {
        return new MatrixOperation(Type.Scale, destinationRow, -1, scalar);
    }
    public static MatrixOperation RowAdd(int sourceRow, int destinationRow, Fraction scalar)
    {
        return new MatrixOperation(Type.Add, destinationRow, sourceRow, scalar);
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Given the row and column in a matrix, get the fraction
    /// that is operating on that fraction (fraction adding or swapping with, etc.)
    /// Returns null if no fraction is operating on that fraction
    /// </summary>
    /// <param name="matrix"></param>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <returns></returns>
    public bool OperatingFraction(Matrix matrix, int row, int col, ref Fraction fraction)
    {
        // Check if we are using a row swap
        if (type == Type.Swap)
        {
            // If this row is the source, we are swapping with the destination
            if (row == sourceRow)
            {
                fraction = matrix.Get(destinationRow, col);
                return true;
            }
            // If this row is the destination, we are swapping with the source
            else if (row == destinationRow)
            {
                fraction = matrix.Get(sourceRow, col);
                return true;
            }
            // If this row is not the source or destination, then no fraction is affecting it
            else return false;
        }
        // Check if this row is the destination
        else if (row == destinationRow)
        {
            // If we are scaling this row then use the scalar fraction
            if (type == Type.Scale)
            {
                fraction = scalar;
                return true;
            }
            // If we are adding to this row, then get the fraction from the source, 
            // scaled by the given amount
            else
            {
                fraction = matrix.Get(sourceRow, col) * scalar;
                return true;
            }
        }
        else return false;
    }
    #endregion
}
