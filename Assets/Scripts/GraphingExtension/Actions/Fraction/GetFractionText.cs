using UnityEngine;
using UnityEngine.Events;

public class GetFractionText : Function<string>
{
    public Input<Fraction> fraction;

    public override string Get()
    {
        return fraction.value.ToString();
    }
}
