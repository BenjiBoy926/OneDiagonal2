using UnityEngine;
using UnityEngine.Events;

public class UpdateEvents : MonoBehaviour
{
    public UnityEvent update;
    public UnityEvent fixedUpdate;
    public UnityEvent lateUpdate;

    private void Update()
    {
        update.Invoke();
    }

    private void FixedUpdate()
    {
        fixedUpdate.Invoke();
    }

    private void LateUpdate()
    {
        lateUpdate.Invoke();
    }
}
