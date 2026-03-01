namespace Utime_utility
{
    public class SD
    {
        public static string date_pattern = @"\d{2}/\d{2}/\d{4}";
        public enum ApiType
        {
            GET,
            POST,
            DELETE,
            PUT
        }
        public static string priority_pattern = @"[A-E]";
        public static Dictionary<string,string> days = new Dictionary<string,string>() { { "Mon", "Пн" }, { "Tue","Вт" }, { "Wed","Ср" }, { "Thu","Чт" }, { "Fri","Пт" }, { "Sat","Сб" }, { "Sun","Вс" } };


    }
}
