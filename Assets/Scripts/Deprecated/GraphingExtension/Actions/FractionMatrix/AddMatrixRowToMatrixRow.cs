using UnityEngine;
using UnityEngine.Events;

public class AddMatrixRowToMatrixRow : SupplierAction<Matrix>
{
    public Input<Matrix> input;
    public Input<int> addingRow;
    public Input<Fraction> rowScalar;
    public Input<int> targetRow;

    public override Matrix Get()
    {
        return input.value.AddRowToRow(addingRow.value, rowScalar.value, targetRow.value);
    }
}
