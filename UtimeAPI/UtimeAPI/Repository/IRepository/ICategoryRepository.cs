using UtimeAPI.Models;
namespace UtimeAPI.Repository.IRepository
{
    public interface ICategoryRepository:IRepository<Category>

    {
        public Task<Category> UpdateAsync(Category entity);
    }
}
