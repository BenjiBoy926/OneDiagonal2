using UnityEngine;

public class Foreach<Type> : Supplier<Type>
{
    // FIELDS
    // public
    public Input<Type[]> items;

    public Result<Type> currentValue;
    public Result<int> currentIndex;

    public ActionEvents outputs;

    private Type current;

    public void Invoke()
    {
        Type[] itemsStored = items.value;

        outputs.start.Invoke();

        for(int i = 0; i < itemsStored.Length; i++)
        {
            // Set current value
            currentValue.value = itemsStored[i];
            current = itemsStored[i];

            // Set current index
            currentIndex.value = i;

            // Invoke step function
            outputs.step.Invoke();
        }

        outputs.stop.Invoke();
    }

    public override Type Get()
    {
        return current;
    }
}
