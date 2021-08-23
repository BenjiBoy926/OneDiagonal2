using UnityEngine;
using UnityEngine.Events;

public class Delay : MonoBehaviour
{
    public Input<float> delay;

    public UnityEvent immediate;
    public UnityEvent output;

    public void Invoke()
    {
        immediate.Invoke();

        CancelInvoke();
        Invoke("DelayedInvoke", delay.value);
    }

    public void InvokeImmediately()
    {
        immediate.Invoke();

        CancelInvoke();
        DelayedInvoke();
    }

    private void DelayedInvoke()
    {
        output.Invoke();
    }
}
