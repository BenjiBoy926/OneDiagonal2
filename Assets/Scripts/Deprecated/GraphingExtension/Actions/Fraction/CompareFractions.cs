using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompareFractions : MonoBehaviour
{
    [Tooltip("First fraction to compare")]
    public Input<Fraction> fraction1;
    [Tooltip("Second fraction to compare")]
    public Input<Fraction> fraction2;

    public CompareEvents events;

    public void Compare()
    {
        if (fraction1.value == fraction2.value)
        {
            events.equalTo.Invoke();
        }
        if (fraction1.value != fraction2.value)
        {
            events.notEqualTo.Invoke();
        }
        if (fraction1.value < fraction2.value)
        {
            events.lessThan.Invoke();
        }
        if (fraction1.value <= fraction2.value)
        {
            events.lessThanOrEqualTo.Invoke();
        }
        if (fraction1.value > fraction2.value)
        {
            events.greaterThan.Invoke();
        }
        if (fraction1.value >= fraction2.value)
        {
            events.greaterThanOrEqualTo.Invoke();
        }
    }
}
