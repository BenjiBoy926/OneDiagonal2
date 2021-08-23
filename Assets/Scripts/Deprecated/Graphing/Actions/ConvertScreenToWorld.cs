using UnityEngine;
using UnityEngine.Events;

public class ConvertScreenToWorld : SupplierAction<Vector3>
{
    public enum ScreenToWorldConversionType
    {
        ScreenToWorld,
        WorldToScreen
    }

    public ScreenToWorldConversionType type;
    public bool normalized;

    public Input<Camera> viewport;
    public Input<Vector3> input;

    public override Vector3 Get()
    {
        switch (type)
        {
            case ScreenToWorldConversionType.ScreenToWorld:
                if (normalized)
                {
                    return viewport.value.ViewportToWorldPoint(input.value);
                }
                else return viewport.value.ScreenToWorldPoint(input.value);

            case ScreenToWorldConversionType.WorldToScreen:
                if (normalized)
                {
                    return viewport.value.WorldToViewportPoint(input.value);
                }
                else return viewport.value.WorldToScreenPoint(input.value);

            default: return viewport.value.ScreenToWorldPoint(input.value);
        }
    }
}
