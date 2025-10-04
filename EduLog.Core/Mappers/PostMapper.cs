using EduLog.CoreLayer.DTOs.PostsDTOs;
using EduLog.CoreLayer.Utilities;
using EduLog.DataLayer.Entities;

namespace EduLog.CoreLayer.Mappers
{
    public class PostMapper
    {
        public static Post MapCreateDtoToPost(CreatePostDto dto)
        {
            return new Post()
            {
                Description = dto.Description,
                CategoryId = dto.CategoryId,
                Slug = dto.Slug.ToSlug(),
                Title = dto.Title,
                UserId = dto.UserId,
                Visits = 0,
                IsDelete = false,
                SubCategoryId = dto.SubCategoryId
            };
        }
        public static PostDto MapToDto(Post post)
        {
            return new PostDto()
            {
                Description = post.Description,
                CategoryId = post.CategoryId,
                Slug = post.Slug,
                Title = post.Title,
                UserFullName = post.User?.FullName,
                Visit = post.Visits,
                CreationDate = post.CreateAt,
                Category = post.Category == null ? null : CategoryMapper.Map(post.Category),
                ImageName = post.ImageName,
                PostId = post.Id,
                SubCategoryId = post.SubCategoryId,
                SubCategory = post.SubCategoryId == null ? null : CategoryMapper.Map(post.SubCategory)
            };
        }
        public static Post EditPost(EditPostDto editDto, Post post)
        {
            post.Description = editDto.Description;
            post.Title = editDto.Title;
            post.CategoryId = editDto.CategoryId;
            post.Slug = editDto.Slug.ToSlug();
            post.SubCategoryId = editDto.SubCategoryId;
            return post;
        }
    }
}