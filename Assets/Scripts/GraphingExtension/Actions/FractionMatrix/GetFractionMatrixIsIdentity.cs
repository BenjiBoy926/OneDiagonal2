using UnityEngine;
using System.Collections;

public class GetFractionMatrixIsIdentity : Function<bool>
{
    public Input<FractionMatrix> input;

    public override bool Get()
    {
        return input.value.isIdentity;
    }
}
