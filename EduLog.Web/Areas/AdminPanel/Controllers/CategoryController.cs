using AutoMapper;
using EduLog.CoreLayer.DTOs.CategoriesDTOs;
using EduLog.CoreLayer.Services;
using EduLog.Web.Areas.AdminPanel.Models.Categories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EduLog.Web.Areas.AdminPanel.Controllers
{
    [Area("adminpanel")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService categoryService;
        private readonly IMapper mapper;
        public CategoryController(ICategoryService categoryService,
                                  IMapper mapper)
        {
            this.categoryService = categoryService;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            CoreLayer.Utilities.OperationResult<List<CategoryDto>> categories = await categoryService.GetAllAsync(new CategoryFilterDto());
            return View(categories.Data);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateViewModel categoryCreateViewModel)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            var result = mapper.Map<CategoryCreateDto>(categoryCreateViewModel);
            
            await categoryService.CreateAsync(result);
            return RedirectToAction("Index");
        }
    }
}