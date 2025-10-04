using AutoMapper;
using EduLog.CoreLayer.DTOs.PostsDTOs;
using EduLog.CoreLayer.Mappers;
using EduLog.CoreLayer.Services.FileManager;
using EduLog.CoreLayer.Utilities;
using EduLog.DataLayer.Context;
using EduLog.DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EduLog.CoreLayer.Services.Posts
{
    public class PostService : IPostService
    {
        private readonly BlogContext _context;
        private readonly IFileManager _fileManger;
        private readonly IMapper _mapper;

        public PostService(BlogContext context,
                           IFileManager fileManger,
                           IMapper mapper)
        {
            _context = context;
            _fileManger = fileManger;
            _mapper = mapper;
        }

        public OperationResult CreatePost(CreatePostDto command)
        {
            if (command.ImageFile == null)
                return OperationResult.Error();
            var post = PostMapper.MapCreateDtoToPost(command);

            if (IsSlugExist(post.Slug))
                return OperationResult.Error("Slug تکراری است");

            post.ImageName = _fileManger.SaveFileAndReturnName(command.ImageFile, Directories.PostImage);
            _context.Posts.Add(post);
            _context.SaveChanges();

            return OperationResult.Success();
        }

        public OperationResult EditPost(EditPostDto command)
        {
            var post = _context.Posts.FirstOrDefault(c => c.Id == command.PostId);
            if (post == null)
                return OperationResult.NotFound();

            var oldImage = post.ImageName;
            var newSlug = command.Slug.ToSlug();

            if (post.Slug != newSlug)
                if (IsSlugExist(newSlug))
                    return OperationResult.Error("Slug تکراری است");

            PostMapper.EditPost(command, post);
            if (command.ImageFile != null)
                post.ImageName = _fileManger.SaveFileAndReturnName(command.ImageFile, Directories.PostImage);

            _context.SaveChanges();

            if (command.ImageFile != null)
                _fileManger.DeleteFile(oldImage, Directories.PostImage);

            return OperationResult.Success();
        }

        public PostDto GetPostBy(int postId)
        {
            var post = _context.Posts
                .Include(c => c.SubCategory)
                .Include(c => c.Category)
                .FirstOrDefault(c => c.Id == postId);
            return PostMapper.MapToDto(post);
        }

        public async Task<PostDto?> GetPostBy(string slug)
        {
            var post = await _context.Posts
                .Include(c => c.SubCategory)
                .Include(c => c.Category)
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Slug == slug);

            if (post is null)
                return null;

            return PostMapper.MapToDto(post);
        }

        public PostFilterDto GetPostsByFilter(PostFilterParams filterParams)
        {
            var result = _context.Posts
                .Include(d => d.Category)
                .Include(d => d.SubCategory)
                .OrderByDescending(d => d.CreateAt)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filterParams.CategorySlug))
                result = result.Where(r => r.Category.Slug.Contains(filterParams.CategorySlug));

            if (!string.IsNullOrWhiteSpace(filterParams.Title))
                result = result.Where(r => r.Title.Contains(filterParams.Title));

            var skip = (filterParams.PageId - 1) * filterParams.Take;
            var pageCount = result.Count() / filterParams.Take;

            return new PostFilterDto()
            {
                Posts = result.Skip(skip).Take(filterParams.Take)
                    .Select(post => PostMapper.MapToDto(post)).ToList(),
                FilterParams = filterParams,
                PageCount = pageCount
            };
        }

        public bool IsSlugExist(string slug)
        {
            return _context.Posts.Any(p => p.Slug == slug.ToSlug());
        }

        public async Task<List<PostDto>> GetRelatedPosts(int categoryId)
        {
            return await _context.Posts
                .Where(p => p.CategoryId == categoryId)
                .OrderByDescending(p => p.CreateAt)
                .Take(6)
                .Select(post => PostMapper.MapToDto(post))
                .ToListAsync();
        }

        public List<PostDto> GetPopularPosts()
        {
            return _context.Posts
                .Include(post => post.User)
                .OrderByDescending(post => post.Visits)
                .Take(4)
                .Select(post => PostMapper.MapToDto(post))
                .ToList();
        }
    }
}