using AutoMapper;
using EduLog.CoreLayer.DTOs.CategoriesDTOs;
using EduLog.CoreLayer.Services;
using EduLog.Web.Areas.AdminPanel.Models.Categories;
using Microsoft.AspNetCore.Mvc;

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
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(CategoryCreateViewModel categoryCreateViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var result = mapper.Map<CategoryCreateDto>(categoryCreateViewModel);

            await categoryService.CreateAsync(result);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await categoryService.GetByIdAsync(id);
            if (category == null || category.Status != CoreLayer.Enums.OperationResultStatus.Success)
            {
                return RedirectToAction("Index");
            }

            var model = mapper.Map<CategoryEditViewModel>(category.Data);

            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(CategoryEditViewModel categoryUpdateViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var result = mapper.Map<CategoryUpdateDto>(categoryUpdateViewModel);

            await categoryService.UpdateAsync(result);
            return RedirectToAction("Index");
        }

        [Route("/adminpanel/catagory/create/{parentid?}")]
        public IActionResult Create(int? parentid)
        {
            return View();
        }

        [HttpPost]
        [Route("/adminpanel/catagory/create/{parentid?}")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(int? parentid, CategoryCreateViewModel categoryCreateViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var result = mapper.Map<CategoryCreateDto>(categoryCreateViewModel);

            await categoryService.CreateAsync(result);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return RedirectToAction("Index");

            await categoryService.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        public IActionResult GetSubCategories(int parentId)
        {
            var categoy = categoryService.GetSubCategories(parentId);

            return new JsonResult(categoy);
        }
    }
}