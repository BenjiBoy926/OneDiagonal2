using UnityEngine;
using UnityEngine.Events;

public class SetLineRendererPosition : MonoBehaviour
{
    public Input<LineRenderer> line;
    public Input<int> index;
    public Input<Vector3> position;

    public UnityEvent output;

    public void Invoke()
    {
        line.value.SetPosition(index.value, position.value);
        output.Invoke();
    }
}
