namespace Core.Extensions
{
    public static class FloatExtensions
    {
        public static float ReMap(this float value, float fromLow, float fromHigh, float toLow, float toHigh) => 
            toLow + (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow);
    }
}