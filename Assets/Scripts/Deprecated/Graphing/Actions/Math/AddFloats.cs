public class AddFloats : SupplierAction<float>
{
    public Input<float> float1;
    public Input<float> float2;

    public override float Get()
    {
        return float1.value + float2.value;
    }
}
