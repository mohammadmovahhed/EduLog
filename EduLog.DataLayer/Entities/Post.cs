using EduLog.DataLayer.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduLog.DataLayer.Entities
{
    public class Post : BaseEntity
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }
        public int Visits { get; set; }

        [Required]
        public string Slug { get; set; }

        public PostStatus Status { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public string ImageName { get; set; }


        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; }

        [ForeignKey(nameof(SubCategoryId))]
        public Category SubCategory { get; set; }

        public ICollection<PostComment> PostComments { get; set; }
    }
}