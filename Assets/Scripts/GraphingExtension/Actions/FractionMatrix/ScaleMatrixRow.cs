using UnityEngine;
using UnityEngine.Events;

public class ScaleMatrixRow : Function<FractionMatrix>
{
    public Input<FractionMatrix> input;
    public Input<int> row;
    public Input<Fraction> scalar;

    protected override FractionMatrix GetValue()
    {
        return input.value.ScaleRow(row.value, scalar.value);
    }
}
