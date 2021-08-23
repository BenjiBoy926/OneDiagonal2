using UnityEngine;
using UnityEngine.Events;

public class SetPosition : MonoBehaviour
{
    public Input<GameObject> input;
    public Input<Vector3> newPosition;

    public UnityEvent output;

    public void Invoke()
    {
        input.value.transform.position = newPosition.value;
        output.Invoke();
    }
}
