namespace EduLog.CoreLayer.DTOs.PostsDTOs
{
    public class PostFilterParams
    {
        public string Title { get; set; }
        public string CategorySlug { get; set; }
        public int PageId { get; set; }
        public int Take { get; set; }
    }
}