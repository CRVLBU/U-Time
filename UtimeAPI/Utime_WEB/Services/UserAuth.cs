using Microsoft.AspNetCore.Identity;
using Utime_utility;
using Utime_WEB.Models;
using Microsoft.AspNetCore.Mvc;
using Utime_WEB.Models.DTO;
using Utime_WEB.Services.IServices;

namespace Utime_WEB.Services
{
    public class UserAuth : BaseService, IUserAuth
    {
        public string path;

        public UserAuth(IConfiguration config, IHttpClientFactory factory) : base(factory)
        {
            path = config.GetValue<string>("ServiceURLs:UtimeAPI");
        }
        public async Task<T> LoginAsync<T>(LoginRequestDTO loginRequest)
        {
            return await SendAsync<T>(new Request
            {
                url = path + "api/UserAuthAPI/login",
                apiType = SD.ApiType.POST,
                Contents = loginRequest
            });

        }

        [HttpPost]
        public async Task<T> RegisterAsync<T>(RegisterRequestDTO registerRequest)
        {
            return await SendAsync<T>(new Request
            {
                url = path + "api/UserAuthAPI/register",
                apiType = SD.ApiType.POST,
                Contents = registerRequest
            });
        }



    }
}
