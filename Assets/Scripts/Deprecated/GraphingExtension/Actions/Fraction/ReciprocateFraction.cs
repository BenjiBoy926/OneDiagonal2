using UnityEngine;
using UnityEngine.Events;

public class ReciprocateFraction : SupplierAction<Fraction>
{
    public Input<Fraction> input;

    public override Fraction Get()
    {
        return input.value.reciprocal;
    }
}
