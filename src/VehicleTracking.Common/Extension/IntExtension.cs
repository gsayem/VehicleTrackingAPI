namespace VehicleTracking.Common.Extension
{
    public static class IntExtension
    {
        public static long HourToMilisecond(this int hour)
        {
            return hour * 60 * 60 * 1000;
        }
        public static long MinuteToMilisecond(this int minute)
        {
            return minute * 60 * 1000;
        }
    }
}
