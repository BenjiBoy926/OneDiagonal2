using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Variable<Type> : MonoBehaviour
{
    // VARIABLES
    [SerializeField]
    [Tooltip("The name of the variable this component represents")]
    private string _variableName;

    [SerializeField]
    [Tooltip("The default value of the variable component")]
    private Type _defaultValue;

    [SerializeField]
    [Tooltip("Event invoked when the value of the component changes")]
    private UnityEvent _onValueChanged;

    // PROPERTIES
    private Type _value;
    public Type value
    {
        get
        {
            return _value;
        }
        set
        {
            _value = value;
            _onValueChanged.Invoke();
        }
    }
    public string variableName
    {
        get
        {
            return _variableName;
        }
    }
    public Type defaultValue
    {
        get
        {
            return _defaultValue;
        }
    }
    public UnityEvent onValueChanged
    {
        get
        {
            return _onValueChanged;
        }
    }

    // FUNCTIONS
    private void Awake()
    {
        _value = defaultValue;
    }
    public void LogValue()
    {
        Debug.Log(ToString());
    }
    public override string ToString()
    {
        return _variableName + "{" + value.ToString() + "}";
    }
}
