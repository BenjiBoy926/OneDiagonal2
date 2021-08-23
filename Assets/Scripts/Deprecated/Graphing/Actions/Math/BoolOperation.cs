public class BoolOperation : SupplierAction<bool>
{
    public BoolOperator type;

    public Input<bool> input1;
    public Input<bool> input2;

    public override bool Get()
    {
        switch (type)
        {
            case BoolOperator.And: return input1.value && input2.value;
            case BoolOperator.Or: return input1.value || input2.value;
            case BoolOperator.Not: return !input1.value;
            default: return input1.value;
        }
    }
}
