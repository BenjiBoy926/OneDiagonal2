using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SetColorAlpha : SupplierAction<Color>
{
    public Input<Color> input;
    public Input<float> newAlpha;

    public override Color Get()
    {
        Color inputValue = input.value;
        return new Color(inputValue.r, inputValue.g, inputValue.b, newAlpha.value);
    }
}
