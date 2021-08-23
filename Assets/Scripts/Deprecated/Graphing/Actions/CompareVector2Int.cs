using UnityEngine;

public class CompareVector2Int : MonoBehaviour
{
    public Input<Vector2Int> input1;
    public Input<Vector2Int> input2;

    public EqualityEvents outputs;

    public void Invoke()
    {
        if (input1.value == input2.value)
        {
            outputs.equal.Invoke();
        }
        else outputs.notEqual.Invoke();
    }
}
