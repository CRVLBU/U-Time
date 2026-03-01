using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;
using Microsoft.EntityFrameworkCore;
using UtimeAPI.Data;
using UtimeAPI.Models;
using UtimeAPI.Repository.IRepository;

namespace UtimeAPI.Repository
{
    public class CategoryRepository:Repository<Category> , ICategoryRepository
    {
        DbSet<Category> _categories;
        
        
        public CategoryRepository(ApplicationDBContext db):base(db)
        {

            _categories = db.Set<Category>();
            
        }

        public  async Task<Category> UpdateAsync(Category entity)
        {
            _categories.Update(entity);
            await SaveChangesAsync();
            return entity;
            

        }
    }
}
