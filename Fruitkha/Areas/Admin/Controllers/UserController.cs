using Business.Services.Admin.User;
using Business.ViewModels.Admin.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fruitkha.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await _userService.GetAllUsersAsync();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var model = await _userService.GetUserAsync(id);

            if (model == null)
                return NotFound();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = await _userService.GetCreateUserVMAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserVM createUserVM)
        {
            var isSucceeded = await _userService.CreateUserAsync(createUserVM);

            if (!isSucceeded)
                return View(createUserVM);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            var model = await _userService.GetUpdateUserVMAsync(id);

            if (model == null)
                return NotFound();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(string id, UpdateUserVM updateUserVM)
        {
            var isSucceeded = await _userService.UpdateUserAsync(id, updateUserVM);

            if (!isSucceeded)
                return View(updateUserVM);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var isSucceeded = await _userService.DeleteUserAsync(id);

            if (!isSucceeded)
                return NotFound();

            return RedirectToAction("Index");
        }
    }
}
