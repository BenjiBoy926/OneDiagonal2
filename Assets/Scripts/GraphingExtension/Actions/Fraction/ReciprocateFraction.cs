using UnityEngine;
using UnityEngine.Events;

public class ReciprocateFraction : MonoBehaviour
{
    public Input<Fraction> input;

    public Result<Fraction> result;

    public UnityEvent output;

    public void Invoke()
    {
        result.value = input.value.reciprocal;
        output.Invoke();
    }
}
