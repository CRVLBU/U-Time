using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using Utime_WEB.Models.DTO;

namespace Utime_WEB.Models.ViewModels
{
    public class ActivityGetViewModel
    {
        public IEnumerable<ActivityDTO> activities_done { get; set; }
        public IEnumerable<ActivityDTO> activities_undone { get; set; }
        public List<List<string>> days { get; set; }
        public ActivityCreateDTO activity_create { get; set; }
        public ActivityUpdateDTO activity_update { get; set; }
  
        public int start_ind { get; set; }
    
        public IEnumerable<SelectListItem> categories { get; set; }
        public  List<char> isPlanned { get; set; }
        public string mode { get; set; }
    }
}
