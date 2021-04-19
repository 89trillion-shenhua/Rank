using System;

public class TimeUtil
{
    public static string SecondsToDhmsString(int seconds)
    {
        TimeSpan ts = new TimeSpan(0, 0, seconds);
        return $"{ts.Days}d {ts.Hours}h {ts.Minutes}m {ts.Seconds}s";
    }
}
