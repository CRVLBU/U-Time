using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Utime_utility.SD;
namespace UtimeAPI.Models
{
    public class Activity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string ActivityName { get; set; }
        [ForeignKey(nameof(Category))]
        public int CategoryID { get; set; }
        public Category Category { get; set; }
        public long Urgency { get; set; }
        public long Imp { get; set; }
        public string Priority { get; set; }
        public DateTime DeadLine { get; set; }
        public DateTime CreationDate { get; set; }
        public TimeSpan TimeFrame { get; set; }
        public string UserName { get; set; }
        public bool isDone { get; set; }
        public bool isExpired { get; set; }
       public string Description { get; set; }


    }
}
