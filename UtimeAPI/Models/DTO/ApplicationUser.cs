using Microsoft.AspNetCore.Identity;
namespace UtimeAPI.Models.DTO
{
    public class ApplicationUser:IdentityUser
    {
        public string Name { get; set; }
    }
}
