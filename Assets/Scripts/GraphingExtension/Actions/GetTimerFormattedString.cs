using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class GetTimerFormattedString : MonoBehaviour
{
    public Input<float> timer;

    public Result<string> result;

    public UnityEvent output;

    public void Invoke()
    {
        TimeSpan time = TimeSpan.FromSeconds(timer.value);
        result.value = time.ToString("mm\\:ss\\.fff");
        output.Invoke();
    }
}
