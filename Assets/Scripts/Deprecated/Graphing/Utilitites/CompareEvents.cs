using UnityEngine;
using UnityEngine.Events;
using System.Collections;

[System.Serializable]
public class CompareEvents
{
    [SerializeField]
    public UnityEvent equalTo;
    [SerializeField]
    public UnityEvent notEqualTo;
    [SerializeField]
    public UnityEvent lessThan;
    [SerializeField]
    public UnityEvent lessThanOrEqualTo;
    [SerializeField]
    public UnityEvent greaterThan;
    [SerializeField]
    public UnityEvent greaterThanOrEqualTo;
}
