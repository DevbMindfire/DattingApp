using AutoMapper;
using DattingApp.API.DTO;
using DattingApp.API.Model;
using Microsoft.EntityFrameworkCore;
namespace DattingApp.API.Helpers
{
    public class AutoMapperProfile :Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User,UserDetailedDTO>()
                .ForMember(dest => dest.PhotoUrls,opt => opt.MapFrom(src => src.Photos.GetEnumerator().Current.Url))
                .ForMember(dest => dest.Age ,opt => 
                    opt.MapFrom(src => src.DateOfBirth.CalculateAge()));;
            CreateMap<User,UserListDetailedDTO>()
                .ForMember(dest => dest.Age ,opt => 
                    opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<Photo,PhotoDetailedDTO>();
        }
        
    }
}