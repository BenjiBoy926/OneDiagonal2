public class FloatComparison : SupplierAction<bool>
{
    public BinaryArithmeticComparison comparison;
    public Input<float> float1;
    public Input<float> float2;

    public override bool Get()
    {
        switch(comparison)
        {
            case BinaryArithmeticComparison.EqualTo: return float1.value == float2.value;
            case BinaryArithmeticComparison.GreaterThan: return float1.value > float2.value;
            case BinaryArithmeticComparison.GreaterThanOrEqualTo: return float1.value >= float2.value;
            case BinaryArithmeticComparison.LessThan: return float1.value < float2.value;
            case BinaryArithmeticComparison.LessThanOrEqualTo: return float1.value <= float2.value;
            case BinaryArithmeticComparison.NotEqual: return float1.value != float2.value;
            default: return false;
        }
    }
}
