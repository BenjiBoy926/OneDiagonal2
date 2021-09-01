
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
}
