using AutoMapper;
using UtimeAPI.Models;
using UtimeAPI.Models.DTO;
namespace UtimeAPI
{
    public class MappingConfig:Profile
    {
        public MappingConfig()
        {
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Category,CategoryCreateDTO>().ReverseMap();
            CreateMap<Category, CategoryUpdateDTO>().ReverseMap();

            CreateMap<Activity, ActivityDTO>().ReverseMap();
            CreateMap<Activity, ActivityCreateDTO>().ReverseMap();
            CreateMap<Activity, ActivityUpdateDTO>().ReverseMap();
            CreateMap<ApplicationUser,UserResponseDTO>().ReverseMap();

        }
        
    }
}
