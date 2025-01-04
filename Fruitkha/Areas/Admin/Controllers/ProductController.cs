using Business.Services.Admin.Product;
using Business.ViewModels.Admin.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fruitkha.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<ActionResult> Index()
        {
            var model = await _productService.GetAllProductAsync();
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Create()
        {
            var model = await _productService.GetCreateProductVMAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateProductVM createProductVM)
        {
            if (await _productService.CreateProductAsync(createProductVM))
                return RedirectToAction("Index");

            return View(createProductVM);
        }

        [HttpGet]
        public async Task<ActionResult> Update(int id)
        {
            var model = await _productService.GetUpdateProductVMAsync(id);
            if (model == null)
                return NotFound();

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Update(int id, UpdateProductVM updateProductVM)
        {
            var result = await _productService.UpdateProductAsync(id, updateProductVM);
            if (result.code == 200)
                return RedirectToAction("Index");

            return result.code switch
            {
                404 => NotFound(),
                400 => View(updateProductVM),
                _ => StatusCode(500)
            };
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            if (await _productService.DeleteProductAsync(id))
                return RedirectToAction("Index");

            return NotFound();
        }
    }
}
