using AutoMapper;
using NewsApp.API.DTOs;
using NewsApp.API.Models;


namespace NewsApp.API.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {

            CreateMap<Category, CategoryDto>().ReverseMap();


            CreateMap<News, NewsDto>()
                .ForMember(dest => dest.AppUserName, opt => opt.MapFrom(src => src.AppUser.UserName))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));


            CreateMap<NewsDto, News>()
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.AppUser, opt => opt.Ignore());

            // Diğer eşleştirmeler
            CreateMap<AppUser, RegisterDto>().ReverseMap();
            CreateMap<AppUser, UserDto>().ReverseMap();
            CreateMap<AppUser, UpdateUserDto>().ReverseMap();
            CreateMap<Comment, CommentDto>().ReverseMap();
        }
    }
}