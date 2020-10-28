using UnityEngine;
using UnityEngine.Events;

public class ReciprocateFraction : Function<Fraction>
{
    public Input<Fraction> input;

    protected override Fraction GetValue()
    {
        return input.value.reciprocal;
    }
}
