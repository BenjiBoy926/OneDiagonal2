using UnityEngine;
using UnityEngine.Events;

public class GetLocalScale : SupplierAction<Vector3>
{
    public Input<GameObject> input;

    public override Vector3 Get()
    {
        return input.value.transform.localScale;
    }
}
