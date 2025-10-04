using EduLog.CoreLayer.DTOs.PostsDTOs;
using EduLog.CoreLayer.Utilities;

namespace EduLog.CoreLayer.Services.Posts
{
    public interface IPostService
    {
        OperationResult CreatePost(CreatePostDto command);
        OperationResult EditPost(EditPostDto command);
        PostDto GetPostBy(int postId);
        Task<PostDto?> GetPostBy(string slug);
        PostFilterDto GetPostsByFilter(PostFilterParams filterParams);
        bool IsSlugExist(string slug);
    }
}