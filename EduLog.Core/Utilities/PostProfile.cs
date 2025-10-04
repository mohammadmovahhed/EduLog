using AutoMapper;
using EduLog.CoreLayer.DTOs.PostsDTOs;
using EduLog.DataLayer.Entities;

namespace EduLog.CoreLayer.Utilities
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            // DTO to Entity
            CreateMap<CreatePostDto, Post>();
            CreateMap<EditPostDto, Post>();
            // Entity to DTO (if needed)
            CreateMap<Post, PostDto>();
        }
    }
}