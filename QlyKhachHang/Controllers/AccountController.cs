using Microsoft.AspNetCore.Mvc;
using QlyKhachHang.Models;
using QlyKhachHang.Services;

namespace QlyKhachHang.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthenticationService _authService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAuthenticationService authService, ILogger<AccountController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        // GET: Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var (success, message) = await _authService.RegisterAsync(model);

            if (success)
            {
                TempData["Success"] = message + ". Vui lòng ??ng nh?p.";
                return RedirectToAction(nameof(Login));
            }
            else
            {
                ModelState.AddModelError("", message);
                return View(model);
            }
        }

        // GET: Account/Login
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var customer = await _authService.LoginAsync(model.UsernameOrEmail, model.Password);

            if (customer == null)
            {
                ModelState.AddModelError("", "Tên ??ng nh?p/email ho?c m?t kh?u không chính xác");
                return View(model);
            }

            // Set session
            HttpContext.Session.SetInt32("CustomerId", customer.CustomerId);
            HttpContext.Session.SetString("CustomerName", customer.CustomerName);
            HttpContext.Session.SetString("CustomerEmail", customer.Email);

            // Update last login
            await _authService.UpdateLastLoginAsync(customer.CustomerId);

            TempData["Success"] = $"Chào m?ng {customer.CustomerName}!";

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        // GET: Account/Profile
        public IActionResult Profile()
        {
            var customerId = HttpContext.Session.GetInt32("CustomerId");

            if (customerId == null)
            {
                return RedirectToAction(nameof(Login));
            }

            return View();
        }

        // GET: Account/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["Success"] = "??ng xu?t thành công";
            return RedirectToAction("Index", "Home");
        }
    }
}
