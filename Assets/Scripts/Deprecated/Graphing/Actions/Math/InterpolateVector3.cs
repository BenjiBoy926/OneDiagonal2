using UnityEngine;

public class InterpolateVector3 : Interpolate<Vector3>
{
    protected override Vector3 Lerp(Vector3 a, Vector3 b, float t)
    {
        return Vector3.Lerp(a, b, t);
    }
}
