using UnityEngine;
using UnityEngine.Events;

public class SwapMatrixRows : Function<FractionMatrix>
{
    public Input<FractionMatrix> input;
    public Input<int> row1;
    public Input<int> row2;

    protected override FractionMatrix GetValue()
    {
        return input.value.SwapRows(row1.value, row2.value);
    }
}
