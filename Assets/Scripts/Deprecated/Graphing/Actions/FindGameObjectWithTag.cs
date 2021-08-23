using UnityEngine;
using UnityEngine.Events;

public class FindGameObjectWithTag : SupplierAction<GameObject>
{
    public Input<Tag> tagInput;

    public override GameObject Get()
    {
        return GameObject.FindGameObjectWithTag(tagInput.value.value);
    }
}
