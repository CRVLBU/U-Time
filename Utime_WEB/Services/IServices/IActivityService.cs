using Utime_WEB.Models.DTO;
using Utime_WEB.Models;

namespace Utime_WEB.Services.IServices
{
    public interface IActivityService:IBaseService
    {
        public Task<T> GetAsync<T>(int id,string token);
        public Task<T> GetAllAsync<T>(string token);
        public Task<T> DeleteAsync<T>(int id,string token);
        public Task<T> UpdateAsync<T>(ActivityUpdateDTO categoryDTO, string token);
        public Task<T> CreateAsync<T>(ActivityCreateDTO categoryDTO, string token);
    }
}
