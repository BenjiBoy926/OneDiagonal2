using UnityEngine;
using UnityEngine.Events;

public class AddMatrixRowToMatrixRow : Function<FractionMatrix>
{
    public Input<FractionMatrix> input;
    public Input<int> addingRow;
    public Input<Fraction> rowScalar;
    public Input<int> targetRow;

    protected override FractionMatrix GetValue()
    {
        return input.value.AddRowToRow(addingRow.value, rowScalar.value, targetRow.value);
    }
}
