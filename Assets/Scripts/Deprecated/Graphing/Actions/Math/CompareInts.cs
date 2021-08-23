using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class CompareInts : MonoBehaviour
{
    [Tooltip("First integer to compare")]
    public Input<int> integer1;
    [Tooltip("Second integer to compare")]
    public Input<int> integer2;

    public CompareEvents events;

    public void Compare()
    {
        if(integer1.value == integer2.value)
        {
            events.equalTo.Invoke();
        }
        if (integer1.value != integer2.value)
        {
            events.notEqualTo.Invoke();
        }
        if (integer1.value < integer2.value)
        {
            events.lessThan.Invoke();
        }
        if (integer1.value <= integer2.value)
        {
            events.lessThanOrEqualTo.Invoke();
        }
        if (integer1.value > integer2.value)
        {
            events.greaterThan.Invoke();
        }
        if (integer1.value >= integer2.value)
        {
            events.greaterThanOrEqualTo.Invoke();
        }
    }
}
