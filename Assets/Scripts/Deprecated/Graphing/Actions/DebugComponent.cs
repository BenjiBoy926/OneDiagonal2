using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DebugComponent : MonoBehaviour
{
    public List<StringOrObject> objects;

    public UnityEvent output;

    public void Log()
    {
        System.Text.StringBuilder builder = new System.Text.StringBuilder();

        foreach(StringOrObject obj in objects)
        {
            builder.Append(obj.ToString());
        }

        Debug.Log(builder.ToString());
    }
}
