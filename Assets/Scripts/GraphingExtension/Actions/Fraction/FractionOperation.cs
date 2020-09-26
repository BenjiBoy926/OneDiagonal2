public class FractionOperation : SupplierAction<Fraction>
{
    public BinaryArithmeticOperation operation;
    public Input<Fraction> _operator;
    public Input<Fraction> operand;

    public override Fraction Get()
    {
        switch(operation)
        {
            case BinaryArithmeticOperation.Add: return _operator.value + operand.value;
            case BinaryArithmeticOperation.Subtract: return _operator.value - operand.value;
            case BinaryArithmeticOperation.Multiply: return _operator.value * operand.value;
            case BinaryArithmeticOperation.Divide: return _operator.value / operand.value;
            default: return _operator.value;
        }
    }
}
