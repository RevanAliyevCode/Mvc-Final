using Business.Services.Cart;
using Domain.Configs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Fruitkha.Controllers
{
    [Authorize(Roles = "Seller, Customer")]
    public class CartController : Controller
    {
        readonly ICartService _cartService;
        readonly StripeConfiguration _stripeConfiguration;

        public CartController(ICartService cartService, IOptions<StripeConfiguration> stripeConfiguration)
        {
            _cartService = cartService;
            _stripeConfiguration = stripeConfiguration.Value;
        }

        public async Task<ActionResult> Index()
        {
            ViewBag.StripePublishableKey = _stripeConfiguration.PublishableKey;
            var basket = await _cartService.GetBasketVMAsync();

            return View(basket);
        }

    }
}
