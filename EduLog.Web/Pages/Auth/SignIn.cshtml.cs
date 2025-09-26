using EduLog.CoreLayer.DTOs.UserDTOs;
using EduLog.CoreLayer.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace EduLog.Web.Pages.Auth
{
    [BindProperties]
    [ValidateAntiForgeryToken]
    public class SignInModel : PageModel
    {
        #region Properties
        [Required(ErrorMessage = "نام کاربری الزامی است.")]
        [Display(Name = "نام کاربری")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "کلمه عبور الزامی است.")]
        [Display(Name = "کلمه عبور")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "مرا به خاطر بسپار")]
        public bool RememberMe { get; set; }
        #endregion

        private readonly IUserService _userService;
        private readonly ILogger<SignInModel> _logger;
        public SignInModel(IUserService userService, ILogger<SignInModel> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            try
            {
                var result = await _userService.LoginUser(new UserLoginDto
                {
                    UserName = UserName,
                    Password = Password
                });

                if (result.Status != CoreLayer.Enums.OperationResultStatus.Success || result.Data is null)
                {
                    ModelState.AddModelError(string.Empty, result.Message);
                    _logger.LogWarning("تلاش ناموفق برای ورود با نام کاربری: {UserName}", UserName);
                    return Page();
                }

                var claims = new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, result.Data.Id.ToString()),
                    new(ClaimTypes.Name, result.Data.FullName),
                    new(ClaimTypes.GivenName, result.Data.UserName),
                    new(ClaimTypes.Role, result.Data.Role.ToString())
                };
                var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimPrinciple = new ClaimsPrincipal(claimIdentity);
                var properties = new AuthenticationProperties()
                {
                    IsPersistent = RememberMe,
                    ExpiresUtc = RememberMe ? DateTimeOffset.UtcNow.AddDays(7) : DateTimeOffset.UtcNow.AddHours(2),
                    AllowRefresh = true
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimPrinciple, properties);

                _logger.LogInformation("کاربر {UserName} با موفقیت وارد شد", UserName);

                return RedirectToPage("../index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در هنگام ورود کاربر {UserName}", UserName);
                ModelState.AddModelError(string.Empty, "خطایی در سیستم رخ داده است");
                return Page();
            }
        }
    }
}
