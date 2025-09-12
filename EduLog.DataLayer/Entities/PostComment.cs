using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduLog.DataLayer.Entities
{
    public class PostComment : BaseEntity
    {
        [Required]
        public string Text { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }


        [ForeignKey(nameof(PostId))]
        public Post Post { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
    }
}