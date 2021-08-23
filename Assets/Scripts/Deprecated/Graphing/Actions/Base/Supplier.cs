using UnityEngine;
using UnityEngine.Events;

public class Supplier<TResult> : MonoBehaviour
{
    public virtual TResult Get()
    {
        throw new System.NotImplementedException("Subclass of Supplier named " + GetType().Name + 
            " does not override base Get() method where overriding is required");
    }
}
