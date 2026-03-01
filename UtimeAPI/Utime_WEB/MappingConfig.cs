using AutoMapper;
using Utime_WEB.Models.DTO;
using Utime_WEB.Models;
namespace Utime_WEB
{
    public class MappingConfig:Profile
    {
        public MappingConfig()
        {
            CreateMap<CategoryCreateDTO, CategoryUpdateDTO>().ReverseMap();
            CreateMap<ActivityDTO, ActivityUpdateDTO>().ReverseMap();
        }
        
    }
}
