using Business.Services.Shop;
using Microsoft.AspNetCore.Mvc;

namespace Fruitkha.Controllers
{
    public class ShopController : Controller
    {
        readonly IShopService _shopService;

        public ShopController(IShopService shopService)
        {
            _shopService = shopService;
        }

        public async Task<ActionResult> Index()
        {
            var model = await _shopService.GetIndexVMAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> FilterByCategory(int categoryId)
        {
            return PartialView("_ProductPartial", await _shopService.FilterByCategory(categoryId));
        }

    }
}
