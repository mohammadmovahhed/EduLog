using EduLog.CoreLayer.DTOs.CategoriesDTOs;
using EduLog.CoreLayer.Utilities;
using EduLog.DataLayer.Context;
using EduLog.DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace EduLog.CoreLayer.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly BlogContext _blogContext;
        public CategoryService(BlogContext blogContext)
        {
            _blogContext = blogContext;
        }

        public async Task<OperationResult> GetByIdAsync(int id)
        {
            if (id <= 0)
                return OperationResult.Error("آیدی معتبر نیست");

            var category = await _blogContext.Categories
                .Where(c => c.Id == id && !c.IsDelete)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Slug = c.Slug,
                    MetaTag = c.MetaTag,
                    MetaDescription = c.MetaDescription,
                    ParentId = c.ParentId,
                    CreateAt = c.CreateAt,
                    PostsCount = c.Posts.Count(p => !p.IsDelete)
                })
                .FirstOrDefaultAsync();

            if (category == null)
                return OperationResult.NotFound("دسته‌بندی یافت نشد");

            return OperationResult.Success(category);
        }

        public async Task<OperationResult> GetBySlugAsync(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
                return OperationResult.Error("Slug معتبر نیست");

            var category = await _blogContext.Categories
                .Where(c => c.Slug == slug.Trim() && !c.IsDelete)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Slug = c.Slug,
                    MetaTag = c.MetaTag,
                    MetaDescription = c.MetaDescription,
                    ParentId = c.ParentId,
                    CreateAt = c.CreateAt,
                    PostsCount = c.Posts.Count(p => !p.IsDelete)
                })
                .FirstOrDefaultAsync();

            if (category == null)
                return OperationResult.NotFound("دسته‌بندی یافت نشد");

            return OperationResult.Success(category);
        }

        public async Task<OperationResult<List<CategoryDto>>> GetAllAsync(CategoryFilterDto filter)
        {
            var query = _blogContext.Categories
                .Where(c => !c.IsDelete)
                .AsQueryable();

            // فیلترها
            if (!string.IsNullOrWhiteSpace(filter.Title))
                query = query.Where(c => c.Title.Contains(filter.Title.Trim()));

            if (!string.IsNullOrWhiteSpace(filter.Slug))
                query = query.Where(c => c.Slug.Contains(filter.Slug.Trim()));

            // مرتب‌سازی
            query = query.OrderByDescending(c => c.CreateAt);

            // صفحه‌بندی
            var categories = await query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Slug = c.Slug,
                    MetaTag = c.MetaTag,
                    MetaDescription = c.MetaDescription,
                    ParentId = c.ParentId,
                    CreateAt = c.CreateAt,
                    PostsCount = filter.IncludePostsCount ? c.Posts.Count(p => !p.IsDelete) : 0
                })
                .ToListAsync();

            return OperationResult<List<CategoryDto>>.Success(categories);
        }

        public async Task<OperationResult> CreateAsync(CategoryCreateDto createDto)
        {
            // اعتبارسنجی
            var validationResult = await ValidateCategoryAsync(createDto.Title, createDto.Slug, createDto.ParentId);
            if (!validationResult.IsSuccess)
                return OperationResult.Error(validationResult.Message);

            // بررسی یکتا بودن Slug
            var isSlugUnique = await IsSlugUniqueAsync(createDto.Slug);
            if (!isSlugUnique.Data)
                return OperationResult.Error("این Slug قبلاً استفاده شده است");

            // ایجاد دسته‌بندی جدید
            var category = new Category
            {
                Title = createDto.Title.Trim(),
                Slug = createDto.Slug.Trim().ToLower(),
                MetaTag = createDto.MetaTag?.Trim(),
                MetaDescription = createDto.MetaDescription?.Trim(),
                ParentId = createDto.ParentId,
                CreateAt = DateTime.Now,
                IsDelete = false
            };

            try
            {
                await _blogContext.Categories.AddAsync(category);
                await _blogContext.SaveChangesAsync();

                var createdCategory = await GetByIdAsync(category.Id);
                return createdCategory;
            }
            catch (DbUpdateException ex)
            {
                return OperationResult.Error("خطا در ایجاد دسته‌بندی");
            }
        }

        public async Task<OperationResult> UpdateAsync(CategoryUpdateDto updateDto)
        {
            // اعتبارسنجی
            var validationResult = await ValidateCategoryAsync(updateDto.Title, updateDto.Slug, updateDto.ParentId, updateDto.Id);
            if (!validationResult.IsSuccess)
                return OperationResult.Error(validationResult.Message);

            // پیدا کردن دسته‌بندی موجود
            var category = await _blogContext.Categories
                .FirstOrDefaultAsync(c => c.Id == updateDto.Id && !c.IsDelete);

            if (category == null)
                return OperationResult.NotFound("دسته‌بندی یافت نشد");

            // بررسی یکتا بودن Slug
            var isSlugUnique = await IsSlugUniqueAsync(updateDto.Slug, updateDto.Id);
            if (!isSlugUnique.Data)
                return OperationResult.Error("این Slug قبلاً استفاده شده است");

            // بررسی حلقه در سلسله مراتب
            if (updateDto.ParentId.HasValue && await HasCircularReferenceAsync(updateDto.Id, updateDto.ParentId.Value))
                return OperationResult.Error("امکان ایجاد حلقه در سلسله مراتب وجود ندارد");

            // به‌روزرسانی
            category.Title = updateDto.Title.Trim();
            category.Slug = updateDto.Slug.Trim().ToLower();
            category.MetaTag = updateDto.MetaTag?.Trim();
            category.MetaDescription = updateDto.MetaDescription?.Trim();
            category.ParentId = updateDto.ParentId;

            try
            {
                _blogContext.Categories.Update(category);
                await _blogContext.SaveChangesAsync();

                var updatedCategory = await GetByIdAsync(category.Id);
                return updatedCategory;
            }
            catch (DbUpdateException ex)
            {
                return OperationResult.Error("خطا در به‌روزرسانی دسته‌بندی");
            }
        }

        public async Task<OperationResult> DeleteAsync(int id)
        {
            if (id <= 0)
                return OperationResult.Error("آیدی معتبر نیست");

            var category = await _blogContext.Categories
                .Include(c => c.Posts)
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDelete);

            if (category == null)
                return OperationResult.NotFound("دسته‌بندی یافت نشد");

            // بررسی وجود پست‌های مرتبط
            if (category.Posts.Any(p => !p.IsDelete))
                return OperationResult.Error("امکان حذف دسته‌بندی دارای پست وجود ندارد");

            // حذف نرم
            category.IsDelete = true;

            try
            {
                _blogContext.Categories.Update(category);
                await _blogContext.SaveChangesAsync();

                return OperationResult.Success("دسته‌بندی با موفقیت حذف شد");
            }
            catch (DbUpdateException ex)
            {
                return OperationResult.Error("خطا در حذف دسته‌بندی");
            }
        }

        public async Task<OperationResult<bool>> IsSlugUniqueAsync(string slug, int? excludeId = null)
        {
            var query = _blogContext.Categories
                .Where(c => !c.IsDelete && c.Slug == slug.Trim().ToLower());

            if (excludeId.HasValue)
                query = query.Where(c => c.Id != excludeId.Value);

            var exists = await query.AnyAsync();
            return OperationResult<bool>.Success(!exists);
        }

        public async Task<OperationResult<List<CategoryDto>>> GetHierarchicalAsync()
        {
            var categories = await _blogContext.Categories
                .Where(c => !c.IsDelete)
                .OrderBy(c => c.ParentId)
                .ThenBy(c => c.Title)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Slug = c.Slug,
                    ParentId = c.ParentId,
                    CreateAt = c.CreateAt,
                    PostsCount = c.Posts.Count(p => !p.IsDelete)
                })
                .ToListAsync();

            return OperationResult<List<CategoryDto>>.Success(categories);
        }

        #region Private Methods

        private async Task<OperationResult> ValidateCategoryAsync(string title, string slug, int? parentId, int? currentId = null)
        {
            if (string.IsNullOrWhiteSpace(title))
                return OperationResult.Error("عنوان الزامی است");

            if (string.IsNullOrWhiteSpace(slug))
                return OperationResult.Error("Slug الزامی است");

            if (parentId.HasValue)
            {
                var parentExists = await _blogContext.Categories
                    .AnyAsync(c => c.Id == parentId.Value && !c.IsDelete);

                if (!parentExists)
                    return OperationResult.Error("دسته‌بندی والد معتبر نیست");
            }

            return OperationResult.Success();
        }

        private async Task<bool> HasCircularReferenceAsync(int categoryId, int? parentId)
        {
            if (categoryId == parentId)
                return true;

            var currentParentId = parentId;
            while (currentParentId.HasValue)
            {
                var parent = await _blogContext.Categories
                    .Where(c => c.Id == currentParentId.Value && !c.IsDelete)
                    .Select(c => new { c.ParentId })
                    .FirstOrDefaultAsync();

                if (parent == null)
                    return false;

                if (parent.ParentId == categoryId)
                    return true;

                currentParentId = parent.ParentId;
            }

            return false;
        }

        #endregion
    }
}