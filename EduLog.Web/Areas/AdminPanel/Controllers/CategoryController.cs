using EduLog.CoreLayer.DTOs.CategoriesDTOs;
using EduLog.CoreLayer.Services;
using Microsoft.AspNetCore.Mvc;

namespace EduLog.Web.Areas.AdminPanel.Controllers
{
    [Area("adminpanel")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            CoreLayer.Utilities.OperationResult<List<CategoryDto>> categories = await categoryService.GetAllAsync(new CategoryFilterDto());
            return View(categories.Data);
        }

        public IActionResult Create()
        {
            //ViewBag.Categories = await categoryService.GetAllAsync(new CategoryFilterDto() { });
            return View();
        }
    }
}