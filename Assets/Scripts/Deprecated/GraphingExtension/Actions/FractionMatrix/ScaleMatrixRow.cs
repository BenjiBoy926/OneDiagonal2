using UnityEngine;
using UnityEngine.Events;

public class ScaleMatrixRow : SupplierAction<Matrix>
{
    public Input<Matrix> input;
    public Input<int> row;
    public Input<Fraction> scalar;

    public override Matrix Get()
    {
        return input.value.ScaleRow(row.value, scalar.value);
    }
}
