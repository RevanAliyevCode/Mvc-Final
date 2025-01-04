using Microsoft.AspNetCore.Mvc;

namespace Fruitkha.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Error(int code, string title, string message)
        {
            ViewData["Code"] = code;
            ViewData["Title"] = title;
            ViewData["Message"] = message;
            return View();
        }
    }
}
