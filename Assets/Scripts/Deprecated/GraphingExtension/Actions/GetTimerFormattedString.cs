using System;

public class GetTimerFormattedString : SupplierAction<string>
{
    public Input<float> timer;

    public override string Get()
    {
        TimeSpan time = TimeSpan.FromSeconds(timer.value);
        return time.ToString("mm\\:ss\\.fff");
    }
}
