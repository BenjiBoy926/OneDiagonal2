public class IntComparison : SupplierAction<bool>
{
    public BinaryArithmeticComparison comparison;
    public Input<int> int1;
    public Input<int> int2;

    public override bool Get()
    {
        switch (comparison)
        {
            case BinaryArithmeticComparison.EqualTo: return int1.value == int2.value;
            case BinaryArithmeticComparison.GreaterThan: return int1.value > int2.value;
            case BinaryArithmeticComparison.GreaterThanOrEqualTo: return int1.value >= int2.value;
            case BinaryArithmeticComparison.LessThan: return int1.value < int2.value;
            case BinaryArithmeticComparison.LessThanOrEqualTo: return int1.value <= int2.value;
            case BinaryArithmeticComparison.NotEqual: return int1.value != int2.value;
            default: return false;
        }
    }
}
