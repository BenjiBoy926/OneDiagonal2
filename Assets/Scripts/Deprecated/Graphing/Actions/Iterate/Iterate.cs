using UnityEngine;

public class Iterate : Supplier<int>
{
    // FIELDS
    // Public
    public Input<int> start;
    public Input<int> end;

    public Result<int> current;

    public ActionEvents outputs;

    // Private
    private int currentValue = 0;

    // INTERFACE
    public void Invoke()
    {
        outputs.start.Invoke();

        for(currentValue = start.value; currentValue < end.value; currentValue++)
        {
            current.value = currentValue;
            outputs.step.Invoke();
        }

        outputs.stop.Invoke();
    }

    public override int Get()
    {
        return currentValue;
    }
}
