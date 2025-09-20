using EduLog.CoreLayer.DTOs.UserDTOs;
using EduLog.CoreLayer.Services.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace EduLog.Web.Pages.Auth
{
    [BindProperties]
    [ValidateAntiForgeryToken]
    public class SignInModel : PageModel
    {
        #region Properties
        [Required(ErrorMessage = "نام کاربری الزامی است.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "کلمه عبور الزامی است.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        #endregion

        private readonly IUserService _userService;
        public SignInModel(IUserService userService)
        {
            _userService = userService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            var result = await _userService.LoginUser(new UserLoginDto
            {
                UserName = UserName,
                Password = Password
            });

            if (result.Status != CoreLayer.Enums.OperationResultStatus.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return Page();
            }

            return RedirectToPage("../index");
        }
    }
}
