using UnityEngine;
using UnityEngine.Events;

public class GetFractionInFractionMatrix : MonoBehaviour
{
    [SerializeField]
    private Input<FractionMatrix> matrix;
    [SerializeField]
    private Input<int> row;
    [SerializeField]
    private Input<int> col;

    [SerializeField]
    private FractionVariable result;

    [SerializeField]
    private UnityEvent output;

    public void Invoke()
    {
        if (result != null) result.value = matrix.value.Get(row.value, col.value);
        output.Invoke();
    }
}
