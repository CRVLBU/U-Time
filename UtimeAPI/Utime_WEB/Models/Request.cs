using static Utime_utility.SD;
namespace Utime_WEB.Models
{
    public class Request
    {
        public ApiType apiType { get; set; }
        public string url { get; set; }
        public object Contents { get; set; }
        public string Token { get; set; }
    }
}
