using AutoMapper;
using ServerApp.DTOs;
using ServerApp.Models;

namespace ServerApp.Helpers
{
    public class MapperProfiles : Profile
    {
        public MapperProfiles()
        {
            CreateMap<User, UserForListDTO>()
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Images.FirstOrDefault(i => i.IsProfile).Name))
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            // dest => destination | opt => options | src => source

            CreateMap<User, UserForDetailsDTO>()
            .ForMember(dest => dest.ProfileImageUrl, opt => opt.MapFrom(src => src.Images.FirstOrDefault(i => i.IsProfile).Name))
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images.Where(i => !i.IsProfile)))
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));

            CreateMap<Image, ImagesForDetailsDTO>();

            CreateMap<UserForUpdateDTO, User>();

            CreateMap<MessageForCreateDTO, Message>().ReverseMap();
        }
    }
}