namespace EduLog.CoreLayer.DTOs.CommentsDTOs
{
    public class CreateCommentDto
    {
        public string Text { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
    }
}