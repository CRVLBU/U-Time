using UtimeAPI.Models;

namespace UtimeAPI.Repository.IRepository
{
    public interface IActivityRepository:IRepository<Activity>
    {
        public Task<Activity> UpdateAsync(Activity entity);
 
    }
}
