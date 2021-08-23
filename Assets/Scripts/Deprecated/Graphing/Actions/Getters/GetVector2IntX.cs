using UnityEngine;
using System.Collections;

public class GetVector2IntX : SupplierAction<int>
{
    public Input<Vector2Int> input;

    public override int Get()
    {
        return input.value.x;
    }
}
