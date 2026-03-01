using Utime_WEB.Models;

namespace Utime_WEB.Services.IServices
{
    public interface IBaseService
    {
        
        public Task<T> SendAsync<T>(Request request);
       
    }
}
