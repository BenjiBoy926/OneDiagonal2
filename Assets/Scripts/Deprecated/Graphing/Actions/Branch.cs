using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Branch : MonoBehaviour
{
    public Input<bool> condition;

    public BoolEvents events;

    public void Invoke()
    {
        if (condition.value)
        {
            events._true.Invoke();
        }
        else events._false.Invoke();
    }
}
