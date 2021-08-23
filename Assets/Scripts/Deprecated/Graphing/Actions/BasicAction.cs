using UnityEngine;
using UnityEngine.Events;

public class BasicAction : MonoBehaviour
{
    public UnityEvent output;

    public void Invoke()
    {
        output.Invoke();
    }
}
