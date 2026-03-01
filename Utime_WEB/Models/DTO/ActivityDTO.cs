using static Utime_utility.SD;
using System.ComponentModel.DataAnnotations.Schema;

namespace Utime_WEB.Models.DTO
{
    public class ActivityDTO
    {
        public int ID { get; set; }
        public string ActivityName { get; set; }
        public CategoryDTO Category { get; set; }
        public int CategoryID { get; set; }
         public int percentage { get; set; }
        public long Urgency { get; set; }
        public long Imp { get; set; }
        public DateTime CreationDate { get; set; }
        public string Priority { get; set; }
        public TimeSpan TimeFrame { get; set; }
        public DateTime DeadLine { get; set; }
        public string UserName { get; set; }
        public bool isDone { get; set; }
        public bool isExpired { get; set; }
        public long opacity { get; set; }
        public string Description { get; set; }
    }
}
