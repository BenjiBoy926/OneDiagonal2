using UnityEngine;
using System.Collections;

[System.Serializable]
public class FractionMatrixReference : Reference<FractionMatrix>
{
    [SerializeField]
    [Tooltip("Type of reference")]
    private ReferenceType type;

    [SerializeField]
    [Tooltip("Component used to get the int, in the case of a variable reference type")]
    private FractionMatrixVariable component = null;

    [SerializeField]
    [Tooltip("Value in the reference")]
    private FractionMatrix value;

    public override FractionMatrix GetValue()
    {
        switch (type)
        {
            case ReferenceType.Value: return value;
            case ReferenceType.Reference: return component.GetValue();
            default: return value;
        }
    }

    public override Variable<FractionMatrix> GetReference()
    {
        return component;
    }
}
