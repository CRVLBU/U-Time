using AutoMapper;
using Utime_WEB.Models;
using Utime_WEB.Models.DTO;
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
