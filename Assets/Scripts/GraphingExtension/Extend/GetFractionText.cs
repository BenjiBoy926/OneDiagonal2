using UnityEngine;
using UnityEngine.Events;

public class GetFractionText : MonoBehaviour
{
    public Input<Fraction> fraction;

    public Result<string> result;

    public UnityEvent output;

    public void Invoke()
    {
        result.value = fraction.value.ToString();
        output.Invoke();
    }
}
