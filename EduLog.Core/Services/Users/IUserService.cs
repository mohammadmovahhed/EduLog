using EduLog.CoreLayer.DTOs.UserDTOs;
using EduLog.CoreLayer.Utilities;

namespace EduLog.CoreLayer.Services.Users
{
    public interface IUserService
    {
        Task<OperationResult> RegisterUser(UserRegisterDto registerDto);
        Task<OperationResult<UserDto>> LoginUser(UserLoginDto loginDto);
        Task<bool> IsUsernameExistsAsync(string username);
    }
}