using UnityEngine;
using UnityEngine.Events;

public class SwapMatrixRows : MonoBehaviour
{
    public Input<FractionMatrix> input;
    public Input<int> row1;
    public Input<int> row2;

    public Result<FractionMatrix> result;

    public UnityEvent output;

    public void Invoke()
    {
        result.value = input.value.SwapRows(row1.value, row2.value);
        output.Invoke();
    }
}
