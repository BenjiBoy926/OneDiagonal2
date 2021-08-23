using UnityEngine;
using UnityEngine.Events;

public class EventFolder : MonoBehaviour
{
    public UnityEvent input;
    public UnityEvent output;

    public void InvokeInput()
    {
        input.Invoke();
    }

    public void InvokeOutput()
    {
        output.Invoke();
    }
}
