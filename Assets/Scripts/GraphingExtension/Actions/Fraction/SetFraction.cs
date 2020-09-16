using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SetFraction : MonoBehaviour
{
    public Input<Fraction> input;

    public Result<Fraction> result;

    public UnityEvent output;

    public void Invoke()
    {
        result.value = input.value;
        output.Invoke();
    }
}
