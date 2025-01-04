using System.Diagnostics;
using Business.Services.Home;
using Microsoft.AspNetCore.Mvc;

namespace Fruitkha.Controllers;

public class HomeController : Controller
{
    readonly IHomeService _homeService;

    public HomeController(IHomeService homeService)
    {
        _homeService = homeService;
    }
    public async Task<IActionResult> Index()
    {
        var model = await _homeService.GetIndexVMAsync();
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> SubscribeToNewsLetter(string email)
    {
        var result = await _homeService.SubscribeToNewsLetterAsync(email);
    
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> ConfirmEmail(string email, string token)
    {
        var result = await _homeService.ConfirmEmailAsync(email, token);
        if (!result)
            return BadRequest();

        return View("ConfirmPage");
    }

}
