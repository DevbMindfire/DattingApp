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

            CreateMap<Photo, PhotoForReturnDTO>();
            CreateMap<PhotoForCreationDto, Photo>();
            CreateMap<UserRegisterDTO, User>();
            CreateMap<User, UserRegisterDTO>();

            CreateMap<MessageForCreation , Message>().ReverseMap();

            CreateMap<Message,MessagetoReturnDTO>()
                    .ForMember(m => m.SenderPhotoUrl,opt => opt.MapFrom(u => u.Sender.Photos.FirstOrDefault().Url))
                    .ForMember(m => m.RecipientPhotoUrl,opt => opt.MapFrom(u => u.Recipient.Photos.FirstOrDefault().Url));
        }
        
    }
}