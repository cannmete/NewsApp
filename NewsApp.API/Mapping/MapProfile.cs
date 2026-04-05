using AutoMapper;
using NewsApp.API.DTOs;
using NewsApp.API.Models;
using NewsApp.API.DTOs;
using NewsApp.API.Models;

namespace NewsApp.API.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            // ReverseMap() sayesinde hem Model -> DTO hem de DTO -> Model dönüşümü yapılır
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<News, NewsDto>().ReverseMap();
            CreateMap<AppUser, RegisterDto>().ReverseMap();
            CreateMap<AppUser, UserDto>().ReverseMap();
            CreateMap<AppUser, UpdateUserDto>().ReverseMap();
            CreateMap<Comment, CommentDto>().ReverseMap();
        }
    }
}