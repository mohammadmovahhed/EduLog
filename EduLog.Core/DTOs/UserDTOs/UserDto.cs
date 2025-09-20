using EduLog.DataLayer.Enums;

namespace EduLog.CoreLayer.DTOs.UserDTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}