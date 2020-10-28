using System;

public class GetTimerFormattedString : Function<string>
{
    public Input<float> timer;

    protected override string GetValue()
    {
        TimeSpan time = TimeSpan.FromSeconds(timer.value);
        return time.ToString("mm\\:ss\\.fff");
    }
}
