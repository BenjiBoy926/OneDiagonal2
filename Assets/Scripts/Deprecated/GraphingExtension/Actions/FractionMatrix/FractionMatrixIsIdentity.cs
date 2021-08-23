using UnityEngine;

public class FractionMatrixIsIdentity : MonoBehaviour
{
    public Input<Matrix> input;

    public BoolEvents outputs;

    public void Invoke()
    {
        if (input.value.isIdentity)
        {
            outputs._true.Invoke();
        }
        else outputs._false.Invoke();
    }
}
