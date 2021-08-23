using UnityEngine;
using UnityEngine.Events;

public class BreakVector2Int : MonoBehaviour
{
    public Input<Vector2Int> input;

    public Result<int> x;
    public Result<int> y;

    public UnityEvent output;

    public void Invoke()
    {
        x.value = input.value.x;
        y.value = input.value.y;
        output.Invoke();
    }
}
