using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UtimeAPI.Models
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID {get;set;}
        public string CategoryName { get;set;}
        public int Amount { get; set; }
        public DateTime CreatedTime { get;set;}
        public string UserName { get; set; }

    }
}
