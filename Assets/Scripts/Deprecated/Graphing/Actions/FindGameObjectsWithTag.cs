using UnityEngine;
using System.Collections;

public class FindGameObjectsWithTag : SupplierAction<GameObject[]>
{
    public Input<Tag> tagInput;

    public override GameObject[] Get()
    {
        return GameObject.FindGameObjectsWithTag(tagInput.value.value);
    }
}
