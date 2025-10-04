using EduLog.CoreLayer.DTOs.CommentsDTOs;
using EduLog.CoreLayer.Utilities;
using EduLog.DataLayer.Context;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EduLog.CoreLayer.Services.Comment
{
    public class CommentService : ICommentService
    {
        private readonly BlogContext blogContext;
        public CommentService(BlogContext blogContext)
        {
            this.blogContext = blogContext;
        }

        public async Task<OperationResult> CreateComment(CreateCommentDto commentDto)
        {
            var comment = new DataLayer.Entities.PostComment
            {
                PostId = commentDto.PostId,
                Text = commentDto.Text,
                UserId = commentDto.UserId
            };
            await blogContext.PostComments.AddAsync(comment);
            await blogContext.SaveChangesAsync();
            return OperationResult.Success();
        }

        public async Task<List<CommentDto>> GetPostComments(int postId)
        {
            return await blogContext.PostComments.Where(pc => pc.PostId == postId)
                .Include(comment => comment.User)
                .Select(comment => new CommentDto
                {
                    CommentId = comment.Id,
                    PostId = comment.PostId,
                    Text = comment.Text,
                    UserFullName = comment.User.FullName,
                    CreateAt = comment.CreateAt
                })
                .ToListAsync();
        }
    }
}
