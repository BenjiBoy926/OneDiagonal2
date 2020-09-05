using UnityEngine;
using UnityEngine.Events;

public class GetFractionInFractionMatrix : MonoBehaviour
{
    public Input<FractionMatrix> matrix;
    public Input<int> row;
    public Input<int> col;

    public Result<Fraction> result;

    public UnityEvent output;

    public void Invoke()
    {
        if (result != null) result.value = matrix.value.Get(row.value, col.value);
        output.Invoke();
    }
}
