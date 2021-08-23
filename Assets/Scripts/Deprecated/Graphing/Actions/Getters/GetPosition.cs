using UnityEngine;

public class GetPosition : SupplierAction<Vector3>
{
    public Space space;
    public Input<GameObject> input;

    public override Vector3 Get()
    {
        if (space == Space.World)
        {
            return input.value.transform.position;
        }
        else return input.value.transform.localPosition;
    }
}
