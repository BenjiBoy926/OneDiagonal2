
public class GetMatrixDimension : Function<int>
{
    public MatrixDimension dimension;
    public Input<FractionMatrix> matrix;

    public override int Get()
    {
        if (dimension == MatrixDimension.Rows)
        {
            return matrix.value.rows;
        }
        else return matrix.value.cols;
    }
}
