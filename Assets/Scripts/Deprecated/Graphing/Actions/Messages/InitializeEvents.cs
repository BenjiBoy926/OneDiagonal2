using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class InitializeEvents : MonoBehaviour
{
    public UnityEvent awake;
    public UnityEvent start;

    private void Awake()
    {
        awake.Invoke();   
    }
    private void Start()
    {
        start.Invoke();
    }
}
