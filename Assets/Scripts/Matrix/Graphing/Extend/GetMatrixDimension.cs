using UnityEngine;
using UnityEngine.Events;

public class GetMatrixDimension : MonoBehaviour
{
    [SerializeField]
    private MatrixDimensionReference dimension;
    [SerializeField]
    private FractionMatrixReference matrix;

    [SerializeField]
    private IntVariable result;

    [SerializeField]
    private UnityEvent output;

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
