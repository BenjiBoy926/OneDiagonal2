using UnityEngine;
using UnityEngine.Events;

public class GetMatrixDimension : MonoBehaviour
{
    [SerializeField]
    private Input<MatrixDimension> dimension;
    [SerializeField]
    private Input<FractionMatrix> matrix;

    [SerializeField]
    private IntVariable result;

    [SerializeField]
    private UnityEvent output;

    public void Invoke()
    {
        if (dimension.value == MatrixDimension.Rows)
        {
            if (result != null) result.value = matrix.value.rows;
        }
        else if (result != null) result.value = matrix.value.cols;

        output.Invoke();
    }
}
