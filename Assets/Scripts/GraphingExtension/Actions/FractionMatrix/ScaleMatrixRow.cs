using UnityEngine;
using UnityEngine.Events;

public class ScaleMatrixRow : MonoBehaviour
{
    public Input<FractionMatrix> input;
    public Input<int> row;
    public Input<Fraction> scalar;

    public Result<FractionMatrix> result;

    public UnityEvent output;

    public void Invoke()
    {
        result.value = input.value.ScaleRow(row.value, scalar.value);
        output.Invoke();
    }
}
