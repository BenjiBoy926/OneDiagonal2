using UnityEngine;
using UnityEngine.Events;

public class GetDeltaTime : SupplierAction<float>
{
    public override float Get()
    {
        return Time.deltaTime;
    }
}
