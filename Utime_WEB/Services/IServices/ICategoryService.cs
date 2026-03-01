using Utime_WEB.Models;
using Utime_WEB.Models.DTO;

namespace Utime_WEB.Services.IServices
{
    public interface ICategoryService:IBaseService
    {
        public Task<T> GetAsync<T>(int id,string token);
        public Task<T> GetAllAsync<T>(string token);
        public Task<T> DeleteAsync<T>(int id,string token);
        public Task<T> UpdateAsync<T>(CategoryUpdateDTO categoryDTO, string token);
        public Task<T> CreateAsync<T>(CategoryCreateDTO categoryDTO,string token);

    }
}
