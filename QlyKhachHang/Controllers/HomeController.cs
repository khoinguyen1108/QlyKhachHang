using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QlyKhachHang.Data;
using QlyKhachHang.Models;

namespace QlyKhachHang.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var customerId = HttpContext.Session.GetInt32("CustomerId");

            if (customerId.HasValue)
            {
                // N?u ng??i dùng ?ã ??ng nh?p, hi?n th? dashboard
                var customer = await _context.Customers
                    .Include(c => c.Carts)
                    .Include(c => c.Reviews)
                    .Include(c => c.Invoices)
                    .FirstOrDefaultAsync(c => c.CustomerId == customerId.Value);

                var stats = new
                {
                    TotalCustomers = await _context.Customers.CountAsync(),
                    TotalProducts = await _context.Products.CountAsync(),
                    TotalCarts = await _context.Carts.CountAsync(),
                    TotalReviews = await _context.Reviews.CountAsync(),
                    TotalInvoices = await _context.Invoices.CountAsync(),
                    CustomerCarts = customer?.Carts.Count ?? 0,
                    CustomerReviews = customer?.Reviews.Count ?? 0,
                    CustomerInvoices = customer?.Invoices.Count ?? 0,
                    TotalRevenue = await _context.Invoices.SumAsync(i => i.TotalAmount)
                };

                ViewData["Stats"] = stats;
                ViewData["Customer"] = customer;
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
