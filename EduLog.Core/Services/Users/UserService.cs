using EduLog.CoreLayer.DTOs.UserDTOs;
using EduLog.CoreLayer.Utilities;
using EduLog.DataLayer.Context;
using EduLog.DataLayer.Entities;
using EduLog.DataLayer.Enums;
using Microsoft.EntityFrameworkCore;

namespace EduLog.CoreLayer.Services
{
    public class UserService : IUserService
    {
        private readonly BlogContext _context;
        public UserService(BlogContext context)
        {
            _context = context;
        }

        public async Task<OperationResult> RegisterUser(UserRegisterDto registerDto)
        {
            if (registerDto == null)
                return OperationResult.Error("اطلاعات ثبت نام نمی‌تواند خالی باشد");

            // اعتبارسنجی ورودی‌ها
            var validationResult = ValidateUserRegistration(registerDto);
            if (!validationResult.IsSuccess)
                return validationResult;

            bool isUsernameExist = await _context.Users
                .AsNoTracking()
                .AnyAsync(u => u.UserName == registerDto.UserName && !u.IsDelete);

            if (isUsernameExist)
                return OperationResult.Error("نام کاربری تکراری است");

            // ایجاد کاربر جدید
            var user = new User
            {
                UserName = registerDto.UserName.Trim(),
                FullName = registerDto.UserFullName?.Trim(),
                CreateAt = DateTime.Now,
                IsDelete = false,
                UserRole = UserRole.User,
                PasswordHash = registerDto.UserPassword.EncodeToBcrypt()
            };

            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                return OperationResult.Success("ثبت نام با موفقیت انجام شد");
            }
            catch (DbUpdateException ex)
            {
                return OperationResult.Error("خطا در ثبت اطلاعات کاربر");
            }
        }

        public async Task<OperationResult<UserDto>> LoginUser(UserLoginDto loginDto)
        {
            if (loginDto == null)
                return OperationResult.Error<UserDto>(message: "اطلاعات ورود نمی‌تواند خالی باشد");

            // اعتبارسنجی ورودی‌ها
            if (string.IsNullOrWhiteSpace(loginDto.UserName))
                return OperationResult.Error<UserDto>(message: "نام کاربری الزامی است");

            if (string.IsNullOrWhiteSpace(loginDto.Password))
                return OperationResult.Error<UserDto>(message: "گذرواژه الزامی است");

            // پیدا کردن کاربر
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserName == loginDto.UserName.Trim() && !u.IsDelete);

            if (user == null)
            {
                // تأخیر برای جلوگیری از brute force
                await Task.Delay(2000);
                return OperationResult.Error<UserDto>(Enums.OperationResultStatus.NotFound, message: "نام کاربری یا گذرواژه نامعتبر است");
            }

            // بررسی گذرواژه
            if (!user.PasswordHash.CompareWithBcrypt(loginDto.Password))
            {
                await Task.Delay(2000);
                return OperationResult.Error<UserDto>(Enums.OperationResultStatus.Unauthorized, "نام کاربری یا کلمه عبور نامعتبر است");
            }

            // بازگرداندن اطلاعات کاربر
            var userDto = MapToUserDto(user);
            return OperationResult.Success(userDto, "ورود موفقیت‌آمیز بود");
        }

        public async Task<bool> IsUsernameExistsAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return false;

            return await _context.Users
                .AsNoTracking()
                .AnyAsync(u => u.UserName == username.Trim() && !u.IsDelete);
        }

        #region Private Methods
        private OperationResult ValidateUserRegistration(UserRegisterDto registerDto)
        {
            if (string.IsNullOrWhiteSpace(registerDto.UserName))
                return OperationResult.Error("نام کاربری الزامی است");

            if (string.IsNullOrWhiteSpace(registerDto.UserPassword))
                return OperationResult.Error("گذرواژه الزامی است");

            if (registerDto.UserPassword.Length < 6)
                return OperationResult.Error("گذرواژه باید حداقل ۶ کاراکتر باشد");

            if (string.IsNullOrWhiteSpace(registerDto.UserFullName))
                return OperationResult.Error("نام کامل الزامی است");

            return OperationResult.Success();
        }

        private UserDto MapToUserDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                FullName = user.FullName,
                RegistrationDate = user.CreateAt,
                Role = user.UserRole
            };
        }
        #endregion
    }
}