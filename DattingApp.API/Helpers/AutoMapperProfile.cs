using AutoMapper;
using DattingApp.API.DTO;
using DattingApp.API.Model;
using System.Linq;
namespace DattingApp.API.Helpers
{
    public class AutoMapperProfile :Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User,UserDetailedDTO>()
                .ForMember(dest => dest.PhotoUrls,opt => 
                    opt.MapFrom(src => src.Photos.FirstOrDefault(p =>p.IsMain).Url))
                .ForMember(dest => dest.Age ,opt => 
                    opt.MapFrom(src => src.DateOfBirth.CalculateAge()));;
            CreateMap<User,UserListDetailedDTO>()
                .ForMember(dest => dest.PhotoUrls,opt => 
                    opt.MapFrom(src => src.Photos.FirstOrDefault(p =>p.IsMain).Url))
                .ForMember(dest => dest.Age ,opt => 
                    opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<Photo,PhotoDetailedDTO>();

            CreateMap<UpdateUserDTO,User>();
        }
        
    }
}