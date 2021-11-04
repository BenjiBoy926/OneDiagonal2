using UnityEngine;

[System.Serializable]
public class Matrix
{
    // FIELDS
    [SerializeField]
    private Fraction[] data;
    [SerializeField]
    private int _rows = 1;

    // PROPERTIES
    public int size => data.Length;
    public int rows => _rows;
    public int cols => data.Length / _rows;
    public bool isIdentity
    {
        get
        {
            bool isIdentity = true;
            int index = 0;
            Vector2Int index2D;

            while (index < data.Length && isIdentity)
            {
                index2D = MyMath.Index1Dto2D(index, _rows, cols);
                if (index2D.x == index2D.y)
                {
                    isIdentity = data[index] == Fraction.one;
                }
                else isIdentity = data[index] == Fraction.zero;

                index++;
            }

            return isIdentity;
        }
    }

    // CONSTRUCTORS
    public Matrix() { }
    public Matrix(int rows, int cols)
    {
        data = new Fraction[rows * cols];

        for (int i = 0; i < data.Length; i++)
        {
            data[i] = Fraction.zero;
        }

        _rows = rows;
    }
    private Matrix(Fraction[] copyData, int rows, int cols)
    {
        data = new Fraction[rows * cols];

        for(int i = 0; i < data.Length; i++)
        {
            if (i < copyData.Length) data[i] = copyData[i];
            else data[i] = Fraction.zero;
        }

        _rows = rows;
    }
    private Matrix(Matrix copyMatrix) : this(copyMatrix.data, copyMatrix._rows, copyMatrix.cols) { }

    // FACTORIES

    public static Matrix Identity(int size)
    {
        Matrix matrix = new Matrix(size, size);
        for(int i = 0; i < size; i++)
        {
            matrix.Set(i, i, Fraction.one);
        }
        return matrix;
    }

    // GET/SET
    public Fraction Get(int i, int j)
    {
        return data[MyMath.Index2Dto1D(i, j, _rows)];
    }
    // Get an array
    public Fraction[] GetRow(int i)
    {
        Fraction[] row = new Fraction[cols];
        for(int j = 0; j < cols; j++)
        {
            row[j] = Get(i, j);
        }
        return row;
    }
    private void Set(int i, int j, Fraction src)
    {
        data[MyMath.Index2Dto1D(i, j, _rows)] = src;
    }

    // ARITHMETIC

    // Swap two rows in the matrix
    public Matrix SwapRows(int row1, int row2)
    {
        Matrix newMatrix = new Matrix(this);
        Fraction swapper;

        for(int i = 0; i < newMatrix.cols; i++)
        {
            swapper = newMatrix.Get(row1, i);
            newMatrix.Set(row1, i, newMatrix.Get(row2, i));
            newMatrix.Set(row2, i, swapper);
        }

        return newMatrix;
    }
    // Scale a row in the matrix
    public Matrix ScaleRow(int row, Fraction scalar)
    {
        Matrix newMatrix = new Matrix(this);
        for (int i = 0; i < newMatrix.cols; i++)
        {
            newMatrix.Set(row, i, newMatrix.Get(row, i) * scalar);
        }
        return newMatrix;
    }
    // Add a scaled row to another rown and store the rown in the same row
    public Matrix AddRowToRow(int rowAdd, Fraction scalar, int rowTarget)
    {
        Matrix newMatrix = new Matrix(this);

        if (rowAdd != rowTarget)
        {
            for (int i = 0; i < newMatrix.cols; i++)
            {
                newMatrix.Set(rowTarget, i, newMatrix.Get(rowTarget, i) + (newMatrix.Get(rowAdd, i) * scalar));
            }
        }
        
        return newMatrix;
    }
    // Operate on the matrix based on the matrix operation passed in
    public Matrix Operate(MatrixOperation operation)
    {
        return operation.type switch
        {
            MatrixOperation.Type.Swap => SwapRows(operation.sourceRow, operation.destinationRow),
            MatrixOperation.Type.Scale => ScaleRow(operation.destinationRow, operation.scalar),
            MatrixOperation.Type.Add => AddRowToRow(operation.sourceRow, operation.scalar, operation.destinationRow),
            _ => this,
        };
    }

    public override string ToString()
    {
        System.Text.StringBuilder builder = new System.Text.StringBuilder("[ ");

        for(int i = 0; i < data.Length; i++)
        {
            builder.Append(data[i].ToString());

            if(i < data.Length - 1)
            {
                builder.Append(", ");
            }
        }
        builder.Append(" ]");

        return builder.ToString();
    }
}