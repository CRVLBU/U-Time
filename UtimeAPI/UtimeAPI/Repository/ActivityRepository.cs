using Microsoft.EntityFrameworkCore;
using UtimeAPI.Data;
using UtimeAPI.Models;
using UtimeAPI.Repository.IRepository;

namespace UtimeAPI.Repository
{
    public class ActivityRepository : Repository<Activity>, IActivityRepository
    {
        public DbSet<Activity> activities;
        public ActivityRepository(ApplicationDBContext db):base(db)
        {
                activities = db.Set<Activity>();
        }
        public async Task<Activity> UpdateAsync(Activity entity)
        {
            activities.Update(entity);
            await SaveChangesAsync();
            return entity;


        }
    


    }
}
