using UnityEngine;
using UnityEngine.Events;

public class SwapMatrixRows : SupplierAction<Matrix>
{
    public Input<Matrix> input;
    public Input<int> row1;
    public Input<int> row2;

    public override Matrix Get()
    {
        return input.value.SwapRows(row1.value, row2.value);
    }
}
