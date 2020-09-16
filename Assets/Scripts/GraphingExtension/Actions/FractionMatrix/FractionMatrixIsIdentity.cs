using UnityEngine;

public class FractionMatrixIsIdentity : MonoBehaviour
{
    public Input<FractionMatrix> input;

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
