using UnityEngine;
using UnityEngine.Events;

public class Instantiate : SupplierAction<GameObject>
{ 
    public Input<GameObject> prefab;
    public Input<GameObject> parent;
    public Input<Vector3> localOrigin;

    public override GameObject Get()
    {
        Transform cloneParent = parent.value != null ? parent.value.transform : null;
        Vector3 worldPosition = cloneParent != null ? cloneParent.position + localOrigin.value : localOrigin.value;
        return Instantiate(prefab.value, worldPosition, prefab.value.transform.rotation, cloneParent);
    }
}
