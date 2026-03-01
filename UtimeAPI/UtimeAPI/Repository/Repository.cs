using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using UtimeAPI.Data;
using UtimeAPI.Repository.IRepository;

namespace UtimeAPI.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public DbSet<T> values { get;set; }
        private readonly ApplicationDBContext _dbContext;
        public Repository(ApplicationDBContext dBContext)
        {
            _dbContext = dBContext;
            values = _dbContext.Set<T>();

        }
        
        public  async Task CreateAsync(T entity)
        {
            await values.AddAsync(entity);
            await SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            values.Remove(entity);
            await SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
        public async Task<T> GetAsync(Expression<Func<T,bool>>? filter, bool track=true, string? includeProperties=null)
        {
            IQueryable<T> value_list = _dbContext.Set<T>();
           
                if (!track)
                {
                    value_list = value_list.AsNoTracking();
                }
            if (includeProperties != null)
            {
                foreach(var item in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    value_list = value_list.Include(item);
                }
            }
            
            value_list = value_list.Where(filter);
            return await value_list.FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T,bool>>? filter,string? includeProperties=null)
        {
            IQueryable<T> value_list = _dbContext.Set<T>();
            if (filter != null)
            {
                value_list = value_list.Where(filter);
            }
            if (includeProperties != null)
            {
                foreach (var item in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    value_list = value_list.Include(item);
                }
            }

            return await value_list.ToListAsync();
        }

        //public   async Task UpdateAsync(T entity)
        //{
        //    values.Update(entity);
        //    await SaveChangesAsync();
        //}
    }
}
