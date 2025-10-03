using EduLog.CoreLayer.DTOs.CategoriesDTOs;
using EduLog.CoreLayer.Utilities;

namespace EduLog.CoreLayer.Services
{
    public interface ICategoryService
    {
        Task<OperationResult> CreateAsync(CategoryCreateDto createDto);
        Task<OperationResult> DeleteAsync(int id);
        Task<OperationResult<List<CategoryDto>>> GetAllAsync(CategoryFilterDto filter);
        Task<OperationResult<CategoryDto>> GetByIdAsync(int id);
        Task<OperationResult> GetBySlugAsync(string slug);
        Task<OperationResult<List<CategoryDto>>> GetHierarchicalAsync();
        Task<OperationResult<bool>> IsSlugUniqueAsync(string slug, int? excludeId = null);
        Task<OperationResult> UpdateAsync(CategoryUpdateDto updateDto);
    }
}