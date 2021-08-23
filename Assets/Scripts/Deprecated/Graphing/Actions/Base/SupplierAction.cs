using UnityEngine.Events;

public class SupplierAction<TResult> : Supplier<TResult>
{
    public Result<TResult> result;

    public UnityEvent output;

    public void Invoke()
    {
        result.value = Get();
        output.Invoke();
    }
}
