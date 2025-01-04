using Business.Services.Admin.Category;
using Business.ViewModels.Admin.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fruitkha.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var model = await _categoryService.GetCategoriesAsync();
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(CategoryCommandVM createCategoryVM)
        {
            var isSucceeded = await _categoryService.CreateCategoryAsync(createCategoryVM);

            if (!isSucceeded)
                return View(createCategoryVM);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Update(int id)
        {
            var model = await _categoryService.GetCategoryAsync(id);

            if (model == null)
                return NotFound();

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Update(int id, CategoryCommandVM updateCategoryVM)
        {
            var (code, message) = await _categoryService.UpdateCategoryAsync(id, updateCategoryVM);

            return code switch
            {
                404 => NotFound(message),
                400 => View(updateCategoryVM),
                200 => RedirectToAction("Index"),
                _ => View(updateCategoryVM)
            };
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            var isSucceeded = await _categoryService.DeleteCategoryAsync(id);

            if (!isSucceeded)
                return NotFound();

            return RedirectToAction("Index");
        }
    }
}
