using UnityEngine;

[System.Serializable]
public class FractionMatrix
{
    // FIELDS
    [SerializeField]
    private Fraction[] data;
    [SerializeField]
    private int _rows = 1;

    // PROPERTIES
    public int rows
    {
        get
        {
            return _rows;
        }
    }
    public int cols
    {
        get
        {
            return data.Length / _rows;
        }
    }
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
    public FractionMatrix() { }
    public FractionMatrix(int rows, int cols)
    {
        data = new Fraction[rows * cols];

        for (int i = 0; i < data.Length; i++)
        {
            data[i] = Fraction.zero;
        }

        _rows = rows;
    }
    public FractionMatrix(Fraction[] copyData, int rows, int cols)
    {
        data = new Fraction[rows * cols];

        for(int i = 0; i < data.Length; i++)
        {
            if (i < copyData.Length) data[i] = copyData[i];
            else data[i] = Fraction.zero;
        }

        _rows = rows;
    }
    public FractionMatrix(FractionMatrix copyMatrix) : this(copyMatrix.data, copyMatrix._rows, copyMatrix.cols) { }

    // GET/SET
    public Fraction Get(int i, int j)
    {
        return data[MyMath.Index2Dto1D(i, j, _rows)];
    }
    private void Set(int i, int j, Fraction src)
    {
        data[MyMath.Index2Dto1D(i, j, _rows)] = src;
    }

    // ARITHMETIC

    // Swap two rows in the matrix
    public FractionMatrix SwapRows(int row1, int row2)
    {
        FractionMatrix newMatrix = new FractionMatrix(this);
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
    public FractionMatrix ScaleRow(int row, Fraction scalar)
    {
        FractionMatrix newMatrix = new FractionMatrix(this);
        for (int i = 0; i < newMatrix.cols; i++)
        {
            newMatrix.Set(row, i, newMatrix.Get(row, i) * scalar);
        }
        return newMatrix;
    }
    // Add a scaled row to another rown and store the rown in the same row
    public FractionMatrix AddRowToRow(int rowAdd, Fraction scalar, int rowTarget)
    {
        FractionMatrix newMatrix = new FractionMatrix(this);

        if (rowAdd != rowTarget)
        {
            for (int i = 0; i < newMatrix.cols; i++)
            {
                newMatrix.Set(rowTarget, i, newMatrix.Get(rowTarget, i) + (newMatrix.Get(rowAdd, i) * scalar));
            }
        }
        
        return newMatrix;
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