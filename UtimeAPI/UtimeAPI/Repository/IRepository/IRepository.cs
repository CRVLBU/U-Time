using System.Linq.Expressions;
using UtimeAPI.Models;

namespace UtimeAPI.Repository.IRepository
{
    public interface IRepository<T> where T:class
    {
        Task<List<T>> GetAllAsync(Expression<Func<T,bool>>? filter=null,string? includeProperties=null);
        Task<T> GetAsync(Expression<Func<T,bool>>? filter, bool track=true, string? includeProperties = null);
        Task CreateAsync(T entity);
        //Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task SaveChangesAsync();
    }
}
