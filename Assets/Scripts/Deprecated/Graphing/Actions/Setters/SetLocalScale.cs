using UnityEngine;
using UnityEngine.Events;

public class SetLocalScale : MonoBehaviour
{
    public Input<GameObject> input;
    public Input<Vector3> newScale;

    public UnityEvent output;

    public void Invoke()
    {
        input.value.transform.localScale = newScale.value;
        output.Invoke();
    }
}
