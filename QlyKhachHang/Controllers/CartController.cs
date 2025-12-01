using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QlyKhachHang.Data;
using QlyKhachHang.Models;

namespace QlyKhachHang.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CartController> _logger;

        public CartController(ApplicationDbContext context, ILogger<CartController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Cart/Index
        public async Task<IActionResult> Index()
        {
            try
            {
                var carts = await _context.Carts
                    .Include(c => c.Customer)
                    .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                    .OrderByDescending(c => c.CreatedDate)
                    .ToListAsync();

                return View(carts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading carts");
                TempData["Error"] = "Có lỗi khi tải danh sách giỏ hàng";
                return View(new List<Cart>());
            }
        }

        // GET: Cart/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var cart = await _context.Carts
                    .Include(c => c.Customer)
                    .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                    .FirstOrDefaultAsync(m => m.CartId == id);

                if (cart == null)
                {
                    return NotFound();
                }

                return View(cart);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading cart details");
                TempData["Error"] = "Có lỗi khi tải chi tiết giỏ hàng";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Cart/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                ViewData["CustomerId"] = new SelectList(
                    await _context.Customers.OrderBy(c => c.CustomerName).ToListAsync(),
                    "CustomerId",
                    "CustomerName");
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading create form");
                TempData["Error"] = "Có lỗi khi tải form tạo giỏ hàng";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Cart/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    cart.CreatedDate = DateTime.Now;
                    _context.Add(cart);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Tạo giỏ hàng thành công";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating cart");
                    ModelState.AddModelError("", "Có lỗi khi tạo giỏ hàng");
                }
            }

            ViewData["CustomerId"] = new SelectList(
                await _context.Customers.OrderBy(c => c.CustomerName).ToListAsync(),
                "CustomerId",
                "CustomerName",
                cart.CustomerId);
            return View(cart);
        }

        // GET: Cart/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var cart = await _context.Carts.FindAsync(id);
                if (cart == null)
                {
                    return NotFound();
                }

                ViewData["CustomerId"] = new SelectList(
                    await _context.Customers.OrderBy(c => c.CustomerName).ToListAsync(),
                    "CustomerId",
                    "CustomerName",
                    cart.CustomerId);
                return View(cart);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading cart for edit");
                TempData["Error"] = "Có lỗi khi tải giỏ hàng";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Cart/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CartId,CustomerId,CreatedDate")] Cart cart)
        {
            if (id != cart.CartId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    cart.ModifiedDate = DateTime.Now;
                    _context.Update(cart);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Cập nhật giỏ hàng thành công";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    _logger.LogError(ex, "Concurrency error editing cart");
                    if (!CartExists(cart.CartId))
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
                    _logger.LogError(ex, "Error editing cart");
                    ModelState.AddModelError("", "Có lỗi khi cập nhật giỏ hàng");
                }
            }

            ViewData["CustomerId"] = new SelectList(
                await _context.Customers.OrderBy(c => c.CustomerName).ToListAsync(),
                "CustomerId",
                "CustomerName",
                cart.CustomerId);
            return View(cart);
        }

        // GET: Cart/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var cart = await _context.Carts
                    .Include(c => c.Customer)
                    .FirstOrDefaultAsync(m => m.CartId == id);

                if (cart == null)
                {
                    return NotFound();
                }

                return View(cart);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading cart for delete");
                TempData["Error"] = "Có lỗi khi tải giỏ hàng";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Cart/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var cart = await _context.Carts
                    .Include(c => c.CartItems)
                    .FirstOrDefaultAsync(c => c.CartId == id);

                if (cart != null)
                {
                    // Delete cart items first (cascade should handle this, but being explicit)
                    _context.CartItems.RemoveRange(cart.CartItems);
                    _context.Carts.Remove(cart);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Xóa giỏ hàng thành công";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting cart");
                TempData["Error"] = "Có lỗi khi xóa giỏ hàng";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool CartExists(int id)
        {
            return _context.Carts.Any(e => e.CartId == id);
        }
    }
}