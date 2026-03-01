using static Utime_utility.SD;
using System.ComponentModel.DataAnnotations.Schema;

namespace UtimeAPI.Models.DTO
{
    public class ActivityDTO
    {
        public int ID { get; set; }
        public string ActivityName { get; set; }
        public CategoryDTO Category { get; set; }
        public long Urgency { get; set; }
        public long Imp { get; set; }
        public int CategoryID { get; set; }
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
