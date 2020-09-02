using UnityEngine;
using UnityEngine.Events;

public class GetFractionInFractionMatrix : MonoBehaviour
{
    [SerializeField]
    private FractionMatrixReference matrix;
    [SerializeField]
    private IntReference row;
    [SerializeField]
    private IntReference col;

    [SerializeField]
    private FractionVariable result;

    [SerializeField]
    private UnityEvent output;

    public void Invoke()
    {
        result.value = matrix.value.Get(row.value, col.value);
        output.Invoke();
    }
}
