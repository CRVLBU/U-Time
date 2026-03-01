using System.ComponentModel.DataAnnotations;

namespace Utime_WEB.Models.DTO
{
    public class CategoryUpdateDTO
    {
        [Required]
        public int ID { get; set; }
        public string CategoryName { get; set; }
        public string UserName { get; set; }

    }
}
