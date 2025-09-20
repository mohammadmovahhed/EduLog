using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EduLog.Web.Pages.Auth
{
    public class SignOutModel : PageModel
    {
        private readonly ILogger<SignOutModel> _logger;
        public SignOutModel(ILogger<SignOutModel> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> OnGet()
        {
            try
            {
                var userName = User.Identity?.Name;

                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                _logger.LogInformation("کاربر {UserName} با موفقیت خارج شد", userName);

                return RedirectToPage("/Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در هنگام خروج کاربر");
                return RedirectToPage("/Index");
            }
        }

        public async Task<IActionResult> OnPost()
        {
            return await OnGet();
        }
    }
}