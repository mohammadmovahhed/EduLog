namespace EduLog.CoreLayer.DTOs.CategoriesDTOs
{
    public class CategoryFilterDto
    {
        public string Title { get; set; }
        public string Slug { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 15;
        public bool IncludePostsCount { get; set; } = true;
    }
}