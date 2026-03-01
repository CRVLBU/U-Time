using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UtimeAPI.Models;
using UtimeAPI.Models.DTO;
namespace UtimeAPI.Data
{
    public class ApplicationDBContext :IdentityDbContext<ApplicationUser>

    {
        public ApplicationDBContext(DbContextOptions options) : base(options)
        { }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //builder.Entity<Category>().HasData(new Category { ID = 1, CategoryName = "Sport", Amount = 1, CreatedTime = DateTime.Now },
            //    new Category { ID = 2, CategoryName = "Family", Amount = 2, CreatedTime = DateTime.Now },
            //    new Category { ID = 3, CategoryName = "University", Amount = 3, CreatedTime = DateTime.Now });
        }
    }
}
