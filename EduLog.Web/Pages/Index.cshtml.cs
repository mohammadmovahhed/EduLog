using EduLog.CoreLayer.Services.Posts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EduLog.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IPostService _postService;

        public IndexModel(ILogger<IndexModel> logger,
                          IPostService postService)
        {
            _logger = logger;
            _postService = postService;
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnGetPopularPosts()
        {
            return Partial("_PopularPosts", _postService.GetPopularPosts());
        }
    }
}