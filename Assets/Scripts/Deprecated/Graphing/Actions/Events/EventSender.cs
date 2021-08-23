using UnityEngine;
using UnityEngine.Events;

public class EventSender : MonoBehaviour
{
    public Input<string> hookName;
    public Input<EventReceiver> receiver;

    public UnityEvent output;

    public void Invoke()
    {
        receiver.value.Invoke(hookName.value);
        output.Invoke();
    }
}
