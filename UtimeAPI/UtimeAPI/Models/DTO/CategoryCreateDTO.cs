using System.ComponentModel.DataAnnotations;

namespace UtimeAPI.Models.DTO
{
    public class CategoryCreateDTO
    {
        [Required]
        public string CategoryName { get; set; }
        public string UserName { get; set; }

    }
}
