using EduLog.DataLayer.Enums;
using System.ComponentModel.DataAnnotations;

namespace EduLog.DataLayer.Entities
{
    public class User : BaseEntity
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string PasswordHash { get; set; }
        public UserRole UserRole { get; set; }


        public ICollection<Post> Posts { get; set; }
        public ICollection<PostComment> PostComments { get; set; }
    }
}