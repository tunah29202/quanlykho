namespace Common
{
    public static class DateTimeExtention
    {
        private static string _dateFormat = "yyyy-MM-dd";
        private static string _dateTimeFormat = "yyyy-MM-dd hh:mm:ss";
        private static string _dateTimeStampFormat = "yyyyMMddHHmmssffff";
        private static string _dateTimeDatabaseFormat = "yyyy-MM-dd HH:mm:ss.fff";
        private static string _datetimeSampleFormat = "yyyy-MM-dd";

        public static string ToDateTimeStampString(DateTime? dateTime = null)
        {
            return dateTime.HasValue ? dateTime.Value.ToString(_dateTimeStampFormat) : DateTime.Now.ToString(_dateTimeStampFormat);
        }
    }
}