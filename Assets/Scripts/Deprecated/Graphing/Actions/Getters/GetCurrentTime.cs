using UnityEngine;

public class GetCurrentTime : SupplierAction<float>
{
    public override float Get()
    {
        return Time.time;
    }
}
