using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IntToString : SupplierAction<string>
{
    public Input<int> integer;

    public override string Get()
    {
        return integer.value.ToString();
    }
}
