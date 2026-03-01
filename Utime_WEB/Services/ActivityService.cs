using Utime_utility;
using Utime_WEB.Models;
using Utime_WEB.Models.DTO;
using Utime_WEB.Services.IServices;

namespace Utime_WEB.Services
{
    public class ActivityService:BaseService,IActivityService
    {
        public string activityURL;
        public ActivityService(IConfiguration config, IHttpClientFactory client) : base(client)
        {
            activityURL = config.GetValue<string>("ServiceURLs:UtimeAPI");
        }
        public async Task<T> CreateAsync<T>(ActivityCreateDTO activityDTO,string token)
        {
            return await SendAsync<T>(new Request
            {
                url = activityURL + "api/ActivityAPI",
                apiType = SD.ApiType.POST,
                Contents = activityDTO,
                Token = token
            });
        }

        public async Task<T> DeleteAsync<T>(int id,string token)
        {
            return await SendAsync<T>(new Request
            {
                url = activityURL + $"api/ActivityAPI/{id}",
                apiType = SD.ApiType.DELETE,
                Token = token
            });
        }

        public async Task<T> GetAllAsync<T>(string token)
        {
            return await SendAsync<T>(new Request
            {
                url = activityURL + "api/ActivityAPI",
                apiType = SD.ApiType.GET,
                Token = token
            });
        }

        public async Task<T> GetAsync<T>(int id, string token)
        {
            return await SendAsync<T>(new Request
            {
                url = activityURL + $"api/ActivityAPI/{id}",
                apiType = SD.ApiType.GET,
                Token = token
            });
        }

        public async Task<T> UpdateAsync<T>(ActivityUpdateDTO activityDTO, string token)
        {
            return await SendAsync<T>(new Request
            {
                url = activityURL + "api/ActivityAPI",
                apiType = SD.ApiType.PUT,
                Contents = activityDTO,
                Token = token
            }) ;
        }
    }
}

