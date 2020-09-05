using UnityEngine;
using UnityEngine.Events;

public class GetMatrixDimension : MonoBehaviour
{
    public Input<MatrixDimension> dimension;
    public Input<FractionMatrix> matrix;

    public Result<int> result;

    public UnityEvent output;

    public void Invoke()
    {
        if (dimension.value == MatrixDimension.Rows)
        {
            result.value = matrix.value.rows;
        }
        else result.value = matrix.value.cols;

        output.Invoke();
    }
}
