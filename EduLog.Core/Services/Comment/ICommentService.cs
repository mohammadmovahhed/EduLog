using EduLog.CoreLayer.DTOs.CommentsDTOs;
using EduLog.CoreLayer.Utilities;

namespace EduLog.CoreLayer.Services.Comment
{
    public interface ICommentService
    {
        Task<OperationResult> CreateComment(CreateCommentDto commentDto);
        Task<List<CommentDto>> GetPostComments(int postId);
    }
}