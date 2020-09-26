using UnityEngine;
using UnityEngine.Events;

public class GetFractionText : SupplierAction<string>
{
    public Input<Fraction> fraction;

    public override string Get()
    {
        return fraction.value.ToString();
    }
}
