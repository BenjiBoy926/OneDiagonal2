using UnityEngine;

[System.Serializable]
public class FractionMatrix
{
    [SerializeField]
    private Fraction[] data;
    [SerializeField]
    private int rows = 1;
    private int cols
    {
        get
        {
            return data.Length / rows;
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

        this.rows = rows;
    }
    public FractionMatrix(Fraction[] copyData, int rows, int cols)
    {
        data = new Fraction[rows * cols];

        for(int i = 0; i < data.Length; i++)
        {
            if (i < copyData.Length) data[i] = copyData[i];
            else data[i] = Fraction.zero;
        }

        this.rows = rows;
    }
    public FractionMatrix(FractionMatrix copyMatrix) : this(copyMatrix.data, copyMatrix.rows, copyMatrix.cols) { }

    // GET/SET
    public Fraction Get(int i, int j)
    {
        return data[Index(i, j)];
    }
    private void Set(int i, int j, Fraction src)
    {
        data[Index(i, j)] = src;
    }
    private int Index(int i, int j)
    {
        return MyMath.Index(i, j, rows, cols);
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

        for (int i = 0; i < newMatrix.cols; i++)
        {
            newMatrix.Set(rowTarget, i, newMatrix.Get(rowTarget, i) + (newMatrix.Get(rowAdd, i) * scalar));
        }

        return newMatrix;
    }
    // Check if this is the identity matrix
    public bool IsIdentity()
    {
        if (data.GetLength(0) == data.GetLength(1))
        {
            bool isIdentity = true;
            int i = 0;

            while(i < data.GetLength(0) && isIdentity)
            {
                int j = 0;

                while(j < data.GetLength(1) && isIdentity)
                {
                    isIdentity = (i == j && Get(i, j) == Fraction.one) || (i != j && Get(i, j) == Fraction.zero);
                    j++;
                }

                i++;
            }
            
            return isIdentity;
        }
        else return false;
    }
}