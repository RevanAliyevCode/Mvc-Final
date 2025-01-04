using Business.Services.Account;
using Business.ViewModels.Account;
using Microsoft.AspNetCore.Mvc;

namespace Fruitkha.Controllers
{
    public class AccountController : Controller
    {
        readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        #region Register
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(SignupVM signupVM)
        {
            var isSucceeded = await _accountService.RegisterUserAsync(signupVM);
            if (!isSucceeded)
                return View(signupVM);

            return RedirectToAction("Login");
        }

        #endregion

        #region Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginVM loginVM)
        {
            var isSucceeded = await _accountService.LoginUserAsync(loginVM);
            if (!isSucceeded)
                return View(loginVM);

            if (!string.IsNullOrEmpty(loginVM.ReturnUrl) && Url.IsLocalUrl(loginVM.ReturnUrl))
            {
                return Redirect(loginVM.ReturnUrl);
            }

            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region ConfirmEmail
        [HttpGet]
        public async Task<ActionResult> ConfirmEmail(string email, string token)
        {
            var (code, message) = await _accountService.ConfirmEmailAsync(email, token);
            if (code == null)
                return RedirectToAction("Index", "Home");

            return code switch
            {
                404 => RedirectToAction("Index", "Error", new { code, title = "User not found", message }),
                400 => RedirectToAction("Index", "Error", new { code, title = "Invalid token", message }),
                200 => RedirectToAction("Login"),
                _ => RedirectToAction("Index", "Error", new { code, title = "Email can not confirmed", message })
            };
        }
        #endregion

        #region ForgotPassword
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ForgotPassword(string email)
        {
            var isSucceeded = await _accountService.ForgotPasswordAsync(email);
            if (!isSucceeded)
                return View(email);

            return RedirectToAction("Login");
        }
        #endregion

        #region ResetPassword
        [HttpGet]
        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ResetPassword(ResetPasswordVM resetPasswordVM)
        {
            var isSucceeded = await _accountService.ResetPasswordAsync(resetPasswordVM);
            if (!isSucceeded)
                return View(resetPasswordVM);

            return RedirectToAction("Login");
        }
        #endregion

        public async Task<ActionResult> Logout()
        {
            await _accountService.LogoutUserAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
