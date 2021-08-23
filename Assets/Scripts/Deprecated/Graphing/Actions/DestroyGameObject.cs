using UnityEngine;
using UnityEngine.Events;

public class DestroyGameObject : MonoBehaviour
{
    [Tooltip("Game object to be destroyed")]
    public Input<GameObject> objectReference;
    [Tooltip("Amount of time it takes before the object is destoyed")]
    public Input<float> time;

    public UnityEvent output;

    public void Destroy()
    {
        Destroy(objectReference.value);
        output.Invoke();
    }

    public void DestroyWithTime()
    {
        Destroy(objectReference.value, time.value);
        output.Invoke();
    }
}
