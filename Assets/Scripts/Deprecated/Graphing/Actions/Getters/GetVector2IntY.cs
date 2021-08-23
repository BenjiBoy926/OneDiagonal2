using UnityEngine;
using System.Collections;

public class GetVector2IntY : SupplierAction<int>
{
    public Input<Vector2Int> input;

    public override int Get()
    {
        return input.value.y;
    }
}
