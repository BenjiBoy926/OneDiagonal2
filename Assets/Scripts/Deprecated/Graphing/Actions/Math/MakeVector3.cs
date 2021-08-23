using UnityEngine;

public class MakeVector3 : SupplierAction<Vector3>
{
    public Input<float> x;
    public Input<float> y;
    public Input<float> z;

    public override Vector3 Get()
    {
        return new Vector3(x.value, y.value, z.value);
    }
}
