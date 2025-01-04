using Business.Services.Payment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fruitkha.Controllers
{
    [Authorize(Roles = "Admin, Customer, Seller")]
    public class PaymentController : Controller
    {
        readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCheckoutSession()
        {
            var (code, message) = await _paymentService.CreateCheckoutSessionAsync();

            if (code == 200)
                return Json(new { id = message });

            return code switch
            {
                400 => BadRequest(new { error = message }),
                401 => Unauthorized(new { error = message }),
                404 => NotFound(new { error = message }),
                _ => StatusCode(500, new { message }),
            };
        }

        [HttpGet]
        public async Task<IActionResult> Success(Guid trackId)
        {
            var (code, message) = await _paymentService.SuccessAsync(trackId);

            if (code == 200)
                return View();

            return code switch
            {
                400 => BadRequest(new { error = message }),
                401 => Unauthorized(new { error = message }),
                404 => NotFound(new { error = message }),
                _ => StatusCode(500, new { message }),
            };
        }

        [HttpGet]
        public async Task<IActionResult> Cancel(Guid trackId)
        {
            var (code, message) = await _paymentService.CancelAsync(trackId);

            if (code == 200)
                return View();

            return code switch
            {
                400 => BadRequest(new { error = message }),
                401 => Unauthorized(new { error = message }),
                404 => NotFound(new { error = message }),
                _ => StatusCode(500, new { message }),
            };

        }

    }
}
