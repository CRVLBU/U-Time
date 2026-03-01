using System.ComponentModel.DataAnnotations;
using static Utime_utility.SD;

namespace Utime_WEB.Models.DTO
{
    public class ActivityUpdateDTO
    {
        public int ID { get; set; }
        public string ActivityName { get; set; }
        public long Urgency { get; set; }
        public long Imp { get; set; }
        [Required]
        public int CategoryID { get; set; }
        public string Priority { get; set; }
        public string DeadLine { get; set; }
        public TimeSpan TimeFrame { get; set; }
        public DateTime CreationDate { get; set; }
        public string UserName { get; set; }
        public bool isDone { get; set; }
        public bool isCheck { get; set; }
        public bool isExpired { get; set; }
        public string Description { get; set; }
    }
}
