using Utime_WEB.Models.ViewModels;

namespace Utime_WEB.Models.Static
{
    public static class ActivityStatic
    {
        public static DateTime TimeFrame { get; set; }

        public static int id { get; set; }
        public static bool isDone { get; set; }
        public static string Priority { get; set; }
        public static int increment { get; set; }
        public static DateTime def_day { get; set; }
        public static string regime { get; set; }
        public static string sorting { get; set; }
        public static bool isExpired { get; set; }
        public static ActivityGetViewModel VM { get; set; }
        public static int Counter1 {get;set;}
        public static int Counter2 { get; set; }
    }
}
