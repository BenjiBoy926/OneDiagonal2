using UnityEngine;
using UnityEngine.Events;

public class EnableEvents : MonoBehaviour
{
    public UnityEvent enable;
    public UnityEvent disable;

    private void OnEnable()
    {
        enable.Invoke();
    }

    private void OnDisable()
    {
        disable.Invoke();
    }
}
