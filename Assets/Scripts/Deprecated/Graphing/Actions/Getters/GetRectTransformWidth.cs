using UnityEngine;

public class GetRectTransformWidth : SupplierAction<float>
{
    public Input<GameObject> input;

    public override float Get()
    {
        RectTransform rect = input.value.GetComponent<RectTransform>();

        if (rect != null)
        {
            return rect.rect.width;
        }
        else return 0;
    }
}
