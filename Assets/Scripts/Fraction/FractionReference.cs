using UnityEngine;
using System.Collections;

[System.Serializable]
public class FractionReference : Reference<Fraction>
{
    [SerializeField]
    [Tooltip("Type of reference")]
    private ReferenceType type;

    [SerializeField]
    [Tooltip("Component used to get the int, in the case of a variable reference type")]
    private FractionVariable component = null;

    [SerializeField]
    [Tooltip("Value in the reference")]
    private Fraction value;

    public override Fraction GetValue()
    {
        switch (type)
        {
            case ReferenceType.Value: return value;
            case ReferenceType.Reference: return component.GetValue();
            default: return value;
        }
    }

    public override Variable<Fraction> GetReference()
    {
        return component;
    }
}
