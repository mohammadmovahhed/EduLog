using System.ComponentModel.DataAnnotations;

namespace EduLog.DataLayer.Entities
{
    public class Category : BaseEntity
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Slug { get; set; }
        public string MetaTag { get; set; }
        public string MetaDescription { get; set; }


        public ICollection<Post> Posts { get; set; }
    }
}