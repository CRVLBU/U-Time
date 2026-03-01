using Utime_WEB.Services.IServices;
using Utime_WEB.Models;
using Utime_WEB.Models.DTO;
using Utime_utility;

namespace Utime_WEB.Services
{
    public class CategoryService : BaseService, ICategoryService
    {
        public string CategoryURL;
        public CategoryService(IConfiguration config, IHttpClientFactory client) : base(client)
        {
            CategoryURL = config.GetValue<string>("ServiceURLs:UtimeAPI");
        }
        public async Task<T> CreateAsync<T>(CategoryCreateDTO categoryDTO, string token)
        {
            return await SendAsync<T>(new Request
            {
                url = CategoryURL+ "api/CategoryAPI",
                apiType = SD.ApiType.POST,
                Contents = categoryDTO,
                Token = token
            });
        }

        public async Task<T> DeleteAsync<T>(int id, string token)
        {
            return await SendAsync<T>(new Request
            {
                url = CategoryURL+$"api/CategoryAPI/{id}",
                apiType = SD.ApiType.DELETE,
                Token = token
            });
        }

        public async Task<T> GetAllAsync<T>(string token)
        {
            return await SendAsync<T>(new Request
            {
                url = CategoryURL+ "api/CategoryAPI",
                apiType = SD.ApiType.GET,
                Token=token
            });
        }

        public async Task<T> GetAsync<T>(int id,string token)
        {
            return await SendAsync<T>(new Request
            {
                url = CategoryURL+$"api/CategoryAPI/{id}",
                apiType = SD.ApiType.GET,
                Token = token
            });
        }

        public async Task<T> UpdateAsync<T>(CategoryUpdateDTO categoryDTO, string token)
        {
            return await SendAsync<T>(new Request
            {
                url = CategoryURL + "api/CategoryAPI",
                apiType = SD.ApiType.PUT,
                Contents=categoryDTO,
                Token = token
            });
        }
    }
}
