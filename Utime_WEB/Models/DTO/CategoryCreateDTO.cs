using System.ComponentModel.DataAnnotations;

namespace Utime_WEB.Models.DTO
{
    public class CategoryCreateDTO
    {
        [Required]
        public string CategoryName { get; set; }
        public string UserName { get; set; }

    }
}
