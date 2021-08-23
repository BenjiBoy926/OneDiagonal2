using UnityEngine;
using System.Collections;

public class CompareFloats : MonoBehaviour
{
    [Tooltip("First integer to compare")]
    public Input<float> float1;
    [Tooltip("Second integer to compare")]
    public Input<float> float2;

    [SerializeField]
    public CompareEvents events;

    public void Compare()
    {
        if (float1.value == float2.value)
        {
            events.equalTo.Invoke();
        }
        if (float1.value != float2.value)
        {
            events.notEqualTo.Invoke();
        }
        if (float1.value < float2.value)
        {
            events.lessThan.Invoke();
        }
        if (float1.value <= float2.value)
        {
            events.lessThanOrEqualTo.Invoke();
        }
        if (float1.value > float2.value)
        {
            events.greaterThan.Invoke();
        }
        if (float1.value >= float2.value)
        {
            events.greaterThanOrEqualTo.Invoke();
        }
    }
}
