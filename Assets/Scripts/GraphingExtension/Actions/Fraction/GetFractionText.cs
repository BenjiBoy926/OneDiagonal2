using UnityEngine;
using UnityEngine.Events;

public class GetFractionText : Function<string>
{
    public Input<Fraction> fraction;

    protected override string GetValue()
    {
        return fraction.value.ToString();
    }
}
