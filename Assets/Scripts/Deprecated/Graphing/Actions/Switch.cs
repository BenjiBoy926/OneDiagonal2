using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class Switch : MonoBehaviour
{
    public Input<int> startingOutputIndex;
    public List<UnityEvent> outputs;

    private int currentOutput;

    private void Awake()
    {
        currentOutput = startingOutputIndex.value % outputs.Count;
    }

    public void Invoke()
    {
        outputs[currentOutput].Invoke();
        currentOutput = (currentOutput + 1) % outputs.Count;
    }
}
