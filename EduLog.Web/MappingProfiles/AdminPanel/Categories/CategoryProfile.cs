using AutoMapper;
using EduLog.Web.Areas.AdminPanel.Models.Categories;
using EduLog.CoreLayer.DTOs.CategoriesDTOs;

namespace EduLog.Web.MappingProfiles.AdminPanel.Categories
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CategoryCreateDto, CategoryCreateViewModel>().ReverseMap();
            CreateMap<CategoryDto, CategoryEditViewModel>().ReverseMap();
            CreateMap<CategoryUpdateDto, CategoryEditViewModel>().ReverseMap();
        }
    }
}