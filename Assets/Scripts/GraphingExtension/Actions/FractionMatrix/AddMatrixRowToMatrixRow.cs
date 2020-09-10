using UnityEngine;
using UnityEngine.Events;

public class AddMatrixRowToMatrixRow : MonoBehaviour
{
    public Input<FractionMatrix> input;
    public Input<int> addingRow;
    public Input<Fraction> rowScalar;
    public Input<int> targetRow;

    public Result<FractionMatrix> result;

    public UnityEvent output;

    public void Invoke()
    {
        result.value = input.value.AddRowToRow(addingRow.value, rowScalar.value, targetRow.value);
        output.Invoke();
    }
}
