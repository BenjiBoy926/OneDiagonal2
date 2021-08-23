using UnityEngine;
using UnityEngine.Events;

public class RandomInt : SupplierAction<int>
{
    public Input<int> min;
    public Input<int> max;

    public override int Get()
    {
        return Random.Range(min.value, max.value);
    }
}
