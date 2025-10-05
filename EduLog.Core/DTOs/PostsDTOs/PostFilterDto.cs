using EduLog.CoreLayer.Utilities;

namespace EduLog.CoreLayer.DTOs.PostsDTOs
{
    public class PostFilterDto: BasePagination
    {
        public List<PostDto> Posts { get; set; }
        public PostFilterParams FilterParams { get; set; }
    }
}