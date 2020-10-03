using UnityEngine;
using UnityEngine.Events;

public class AddMatrixRowToMatrixRow : Function<FractionMatrix>
{
    public Input<FractionMatrix> input;
    public Input<int> addingRow;
    public Input<Fraction> rowScalar;
    public Input<int> targetRow;

    public override FractionMatrix Get()
    {
        return input.value.AddRowToRow(addingRow.value, rowScalar.value, targetRow.value);
    }
}
