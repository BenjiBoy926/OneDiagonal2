using UnityEngine;
using System.Collections;

public class GetFractionMatrixIsIdentity : Function<bool>
{
    public Input<FractionMatrix> input;

    protected override bool GetValue()
    {
        return input.value.isIdentity;
    }
}
