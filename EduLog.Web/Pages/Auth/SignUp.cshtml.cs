using EduLog.CoreLayer.DTOs.UserDTOs;
using EduLog.CoreLayer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace EduLog.Web.Pages.Auth
{
    [BindProperties]
    [ValidateAntiForgeryToken]
    public class SignUpModel : PageModel
    {
        #region Properties
        [Required(ErrorMessage = "نام کاربری الزامی است.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "نام و نام خانوادگی الزامی است.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "کلمه عبور الزامی است.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "تکرار کلمه عبور الزامی است.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "کلمه‌های عبور باید یکسان باشند.")]
        public string ConfirmPassword { get; set; }
        #endregion

        private readonly IUserService _userService;
        public SignUpModel(IUserService userService)
        {
            _userService = userService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            var result = await _userService.RegisterUser(new UserRegisterDto
            {
                UserName = UserName,
                UserFullName = FullName,
                UserPassword = Password
            });

            if (result.Status != CoreLayer.Enums.OperationResultStatus.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return Page();
            }
            return RedirectToPage("/Auth/SignIn");
        }
    }
}