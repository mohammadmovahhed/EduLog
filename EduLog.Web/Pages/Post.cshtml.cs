using EduLog.CoreLayer.DTOs.CommentsDTOs;
using EduLog.CoreLayer.DTOs.PostsDTOs;
using EduLog.CoreLayer.Services.Comment;
using EduLog.CoreLayer.Services.Posts;
using EduLog.CoreLayer.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace EduLog.Web.Pages
{
    public class PostModel : PageModel
    {
        private readonly IPostService postService;
        private readonly ICommentService commentService;
        public PostModel(IPostService postService,
                         ICommentService commentService)
        {
            this.postService = postService;
            this.commentService = commentService;
        }

        public PostDto Post { get; set; }

        [Required]
        [BindProperty]
        public string Text { get; set; }

        [BindProperty]
        public int PostId { get; set; }

        public List<CommentDto> Comments { get; set; }
        public List<PostDto> RelatedPosts { get; set; }

        public async Task<IActionResult> OnGet(string slug)
        {
            Post = await postService.GetPostBy(slug);
            if (Post is null)
                return NotFound();

            Comments = await commentService.GetPostComments(Post.PostId);
            RelatedPosts = await postService.GetRelatedPosts(Post.SubCategoryId ?? Post.CategoryId);
            await postService.IncreaseVisits(Post.PostId);
            return Page();
        }

        public async Task<IActionResult> OnPost(string slug)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToPage("Post", new { slug });

            if (!ModelState.IsValid)
            {
                Post = await postService.GetPostBy(slug);
                RelatedPosts = await postService.GetRelatedPosts(Post.SubCategoryId ?? Post.CategoryId);
                return Page();
            }

            await commentService.CreateComment(new CreateCommentDto
            {
                PostId = PostId,
                Text = Text,
                UserId = User.GetUserId()
            });

            return RedirectToPage("Post", new { slug });
        }
    }
}
