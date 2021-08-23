using UnityEngine;
using UnityEngine.Events;

public class AddFractions : MonoBehaviour
{
    public Input<Fraction> fraction1;
    public Input<Fraction> fraction2;

    public Result<Fraction> result;

    public UnityEvent output;

    public void Invoke()
    {
        result.value = fraction1.value + fraction2.value;
        output.Invoke();
    }
}
