using Business.Services.Admin.SendNews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fruitkha.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SendNewsController : Controller
    {
        readonly ISendNewsService _sendNewsService;

        public SendNewsController(ISendNewsService sendNewsService)
        {
            _sendNewsService = sendNewsService;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var users = await _sendNewsService.GetUsersVMAsync();
            return View(users);
        }

        [HttpGet]
        public ActionResult SendMail()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SendMail(string subject, string message)
        {
            if (await _sendNewsService.SendMailAsync(subject, message))
                return RedirectToAction("Index");

            return View();
        }
    }
}
