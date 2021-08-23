using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class ScaleVector3 : SupplierAction<Vector3>
{
    public Input<Vector3> input;
    public Input<float> scalar;

    public override Vector3 Get()
    {
        return input.value * scalar.value;
    }
}
