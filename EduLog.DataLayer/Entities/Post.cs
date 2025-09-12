using EduLog.DataLayer.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduLog.DataLayer.Entities
{
    public class Post
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }
        public int Visits { get; set; }

        [Required]
        //[Index(IsUnique = true)]
        public string Slug { get; set; }

        public PostStatus Status { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;

        public int UserId { get; set; }
        public int CategoryId { get; set; }


        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; }

        public ICollection<PostComment> PostComments { get; set; }
    }
}