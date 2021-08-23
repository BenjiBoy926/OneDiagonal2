using UnityEngine;

public class IntOperation : SupplierAction<int>
{
    public BinaryArithmeticOperation operation;
    public Input<int> _operator;
    public Input<int> operand;

    public override int Get()
    {
        switch (operation)
        {
            case BinaryArithmeticOperation.Add: return _operator.value + operand.value;
            case BinaryArithmeticOperation.Subtract: return _operator.value - operand.value;
            case BinaryArithmeticOperation.Multiply: return _operator.value * operand.value;
            case BinaryArithmeticOperation.Divide: return _operator.value / operand.value;
            default: return _operator.value;
        }
    }
}
