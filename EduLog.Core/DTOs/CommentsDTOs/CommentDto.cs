namespace EduLog.CoreLayer.DTOs.CommentsDTOs
{
    public class CommentDto
    {
        public int CommentId { get; set; }
        public string Text { get; set; }
        public int PostId { get; set; }
        public string UserFullName { get; set; }
        public DateTime CreateAt { get; set; }
    }
}