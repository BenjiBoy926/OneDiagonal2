using UnityEngine;
using UnityEngine.Events;

public class MakeVector2Int : SupplierAction<Vector2Int>
{
    public Input<int> x;
    public Input<int> y;

    public override Vector2Int Get()
    {
        return new Vector2Int(x.value, y.value);
    }
}
