using UnityEngine;
using System.Collections;

public class GetFractionMatrixIsIdentity : SupplierAction<bool>
{
    public Input<Matrix> input;

    public override bool Get()
    {
        return input.value.isIdentity;
    }
}
