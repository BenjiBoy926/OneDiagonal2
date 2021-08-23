using UnityEngine;
using System.Collections;

[System.Serializable]
public class StringOrObject
{
    // TYPEDEFS
    public enum StringOrObjectType
    {
        String, Object
    }

    [SerializeField]
    private StringOrObjectType type;
    [SerializeField]
    private string str;
    [SerializeField]
    private Object obj;

    public override string ToString()
    {
        if (type == StringOrObjectType.String)
        {
            return str;
        }
        else return obj.ToString();
    }
}
