public class FloatOperation : SupplierAction<float>
{
    public BinaryArithmeticOperation operation;
    public Input<float> _operator;
    public Input<float> operand;

    public override float Get()
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
