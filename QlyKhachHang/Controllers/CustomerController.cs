using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QlyKhachHang.Data;
using QlyKhachHang.Models;

namespace QlyKhachHang.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ApplicationDbContext context, ILogger<CustomerController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Customer/Index
        public async Task<IActionResult> Index()
        {
            try
            {
                var customers = await _context.Customers.OrderBy(c => c.CustomerName).ToListAsync();
                return View(customers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading customers");
                TempData["Error"] = "Có lỗi khi tải danh sách khách hàng";
                return View(new List<Customer>());
            }
        }

        // GET: Customer/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var customer = await _context.Customers
                    .Include(c => c.Carts)
                    .Include(c => c.Reviews)
                    .Include(c => c.Invoices)
                    .FirstOrDefaultAsync(m => m.CustomerId == id);

                if (customer == null)
                {
                    return NotFound();
                }

                return View(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading customer details");
                TempData["Error"] = "Có lỗi khi tải chi tiết khách hàng";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Customer/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,CustomerName,Phone,Email,Address,City,PostalCode")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    customer.CreatedDate = DateTime.Now;
                    _context.Add(customer);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Thêm khách hàng thành công";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex, "Error creating customer");
                    ModelState.AddModelError("", "Lỗi lưu dữ liệu. Kiểm tra xem email hoặc số điện thoại đã tồn tại chưa.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unexpected error creating customer");
                    ModelState.AddModelError("", "Có lỗi bất ngờ khi thêm khách hàng");
                }
            }
            return View(customer);
        }

        // GET: Customer/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var customer = await _context.Customers.FindAsync(id);
                if (customer == null)
                {
                    return NotFound();
                }
                return View(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading customer for edit");
                TempData["Error"] = "Có lỗi khi tải thông tin khách hàng";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Customer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerId,CustomerName,Phone,Email,Address,City,PostalCode,CreatedDate")] Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    customer.ModifiedDate = DateTime.Now;
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Cập nhật khách hàng thành công";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    _logger.LogError(ex, "Concurrency error editing customer");
                    if (!CustomerExists(customer.CustomerId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error editing customer");
                    ModelState.AddModelError("", "Có lỗi khi cập nhật khách hàng");
                }
            }
            return View(customer);
        }

        // GET: Customer/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var customer = await _context.Customers.FirstOrDefaultAsync(m => m.CustomerId == id);
                if (customer == null)
                {
                    return NotFound();
                }

                return View(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading customer for delete");
                TempData["Error"] = "Có lỗi khi tải thông tin khách hàng";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var customer = await _context.Customers.FindAsync(id);
                if (customer != null)
                {
                    // Check if customer has related data
                    var hasInvoices = await _context.Invoices.AnyAsync(i => i.CustomerId == id);
                    var hasReviews = await _context.Reviews.AnyAsync(r => r.CustomerId == id);
                    var hasCarts = await _context.Carts.AnyAsync(c => c.CustomerId == id);

                    if (hasInvoices || hasReviews || hasCarts)
                    {
                        TempData["Error"] = "Không thể xóa khách hàng vì còn dữ liệu liên quan (Hóa đơn, Đánh giá, Giỏ hàng)";
                        return RedirectToAction(nameof(Index));
                    }

                    _context.Customers.Remove(customer);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Xóa khách hàng thành công";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting customer");
                TempData["Error"] = "Có lỗi khi xóa khách hàng";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }
    }
}