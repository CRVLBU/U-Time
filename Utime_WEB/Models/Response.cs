using System.Net;

namespace Utime_WEB.Models
{
    public class Response
    {
        public List<string> ErrorMessages { get; set; }
        public object Contents { get; set; }
        public HttpStatusCode StatusCode { get;set; }
        public bool IsSuccess { get; set; }
    }
}
