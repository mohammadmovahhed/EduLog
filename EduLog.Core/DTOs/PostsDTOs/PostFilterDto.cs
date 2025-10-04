namespace EduLog.CoreLayer.DTOs.PostsDTOs
{
    public class PostFilterDto
    {
        public int PageCount { get; set; }
        public List<PostDto> Posts { get; set; }
        public PostFilterParams FilterParams { get; set; }
    }
}