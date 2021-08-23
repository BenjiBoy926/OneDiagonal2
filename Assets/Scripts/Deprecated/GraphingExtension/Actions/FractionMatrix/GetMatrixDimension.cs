
public class GetMatrixDimension : SupplierAction<int>
{
    public MatrixDimension dimension;
    public Input<Matrix> matrix;

    public override int Get()
    {
        if (dimension == MatrixDimension.Rows)
        {
            return matrix.value.rows;
        }
        else return matrix.value.cols;
    }
}
