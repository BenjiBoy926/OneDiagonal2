using UnityEngine;

public abstract class Interpolate<Type> : Supplier<Type>
{
    public Input<Type> min;
    public Input<Type> max;
    public Input<float> time;
    public Input<AnimationCurve> function;

    public Result<Type> result;

    public ActionEvents outputs;

    private bool active;
    private float interpolator;

    public void StartSmoothing()
    {
        active = true;
        interpolator = 0;
        outputs.start.Invoke();
    }

    public void StopSmoothing()
    {
        active = false;
        outputs.stop.Invoke();
    }

    private void Update()
    {
        if(active)
        {
            // Update the interpolator and lerp the value and invoke output
            interpolator = Mathf.Clamp01(interpolator + (Time.deltaTime / time.value));
            result.value = Get();
            outputs.step.Invoke();

            active = interpolator < 1.0f;

            if(!active)
            {
                StopSmoothing();
            }
        }
    }

    public override Type Get()
    {
        return Lerp(min.value, max.value, function.value.Evaluate(interpolator));
    }

    protected abstract Type Lerp(Type a, Type b, float t);
}
