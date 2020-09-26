using UnityEngine;
using UnityEngine.Events;

public class SwapMatrixRows : SupplierAction<FractionMatrix>
{
    public Input<FractionMatrix> input;
    public Input<int> row1;
    public Input<int> row2;

    public override FractionMatrix Get()
    {
        return input.value.SwapRows(row1.value, row2.value);
    }
}
