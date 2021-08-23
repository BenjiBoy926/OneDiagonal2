using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class BoolEvents
{
    [Tooltip("Event invoked on return true")]
    public UnityEvent _true;
    [Tooltip("Event invoked on false")]
    public UnityEvent _false;
}
