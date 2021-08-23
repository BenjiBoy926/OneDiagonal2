using UnityEngine;
using UnityEngine.Events;

public class TimeControlledInvoker : MonoBehaviour
{
    public Input<float> time;    // Minimum time between invokations
    public Input<bool> startReady;

    public UnityEvent ready;
    public UnityEvent notReady;

    private float minTimeUntilNextInvokation;

    private void Start()
    {
        if (startReady.value)
        {
            minTimeUntilNextInvokation = 0;
        }
        else minTimeUntilNextInvokation = time.value;
    }

    public void Invoke()
    {
        if(Time.time >= minTimeUntilNextInvokation)
        {
            ready.Invoke();
            minTimeUntilNextInvokation = Time.time + time.value;
        }
        else
        {
            notReady.Invoke();
        }
    }
}
