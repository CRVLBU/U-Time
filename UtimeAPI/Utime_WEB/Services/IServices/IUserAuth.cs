using Utime_WEB.Models;
using Utime_WEB.Models.DTO;

namespace Utime_WEB.Services.IServices
{
    public interface IUserAuth : IBaseService
    {
        public Task<T> LoginAsync<T>(LoginRequestDTO loginRequest);
        public Task<T> RegisterAsync<T>(RegisterRequestDTO registerRequest);
    }
}
