using UnityEngine;

public class CompareGameObjects : MonoBehaviour
{
    public Input<GameObject> input1;
    public Input<GameObject> input2;

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
