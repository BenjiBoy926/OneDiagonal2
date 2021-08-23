using UnityEngine;

public class RandomVector2 : SupplierAction<Vector2>
{
    public Input<Vector2> min;
    public Input<Vector2> max;

    public override Vector2 Get()
    {
        return Get(min.value, max.value);
    }

    public static Vector2 Get(Vector2 min, Vector2 max)
    {
        return new Vector2(Random.Range(min.x, max.x), Random.Range(min.y, max.y));
    }
}
