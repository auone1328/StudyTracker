using AutoMapper;
using WebApplication1.Models.DTOs;
using WebApplication1.Models.Entities;


namespace WebApplication1
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<UserDTO, User>().ReverseMap();
        }
    }
}
