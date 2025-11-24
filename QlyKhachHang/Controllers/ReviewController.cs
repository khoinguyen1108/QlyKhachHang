using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QlyKhachHang.Data;
using QlyKhachHang.Models;

namespace QlyKhachHang.Controllers
{
    public class ReviewController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ReviewController> _logger;

        public ReviewController(ApplicationDbContext context, ILogger<ReviewController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Review/Index
        public async Task<IActionResult> Index()
        {
            try
            {
                var reviews = await _context.Reviews
                    .Include(r => r.Customer)
                    .Include(r => r.Product)
                    .OrderByDescending(r => r.CreatedDate)
                    .ToListAsync();

                return View(reviews);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading reviews");
                TempData["Error"] = "Có l?i khi t?i danh sách ?ánh giá";
                return View(new List<Review>());
            }
        }

        // GET: Review/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var review = await _context.Reviews
                    .Include(r => r.Customer)
                    .Include(r => r.Product)
                    .FirstOrDefaultAsync(m => m.ReviewId == id);

                if (review == null)
                {
                    return NotFound();
                }

                return View(review);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading review details");
                TempData["Error"] = "Có l?i khi t?i chi ti?t ?ánh giá";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Review/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                ViewData["CustomerId"] = new SelectList(
                    await _context.Customers.OrderBy(c => c.CustomerName).ToListAsync(),
                    "CustomerId",
                    "CustomerName");

                ViewData["ProductId"] = new SelectList(
                    await _context.Products.OrderBy(p => p.ProductName).ToListAsync(),
                    "ProductId",
                    "ProductName");

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading create form");
                TempData["Error"] = "Có l?i khi t?i form t?o ?ánh giá";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Review/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,ProductId,Rating,Comment")] Review review)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Check if customer already reviewed this product
                    var existingReview = await _context.Reviews
                        .FirstOrDefaultAsync(r => r.CustomerId == review.CustomerId && r.ProductId == review.ProductId);

                    if (existingReview != null)
                    {
                        ModelState.AddModelError("", "Khách hàng này ?ã ?ánh giá s?n ph?m này r?i");
                    }
                    else
                    {
                        review.CreatedDate = DateTime.Now;
                        _context.Add(review);
                        await _context.SaveChangesAsync();
                        TempData["Success"] = "Thêm ?ánh giá thành công";
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating review");
                    ModelState.AddModelError("", "Có l?i khi thêm ?ánh giá");
                }
            }

            ViewData["CustomerId"] = new SelectList(
                await _context.Customers.OrderBy(c => c.CustomerName).ToListAsync(),
                "CustomerId",
                "CustomerName",
                review.CustomerId);

            ViewData["ProductId"] = new SelectList(
                await _context.Products.OrderBy(p => p.ProductName).ToListAsync(),
                "ProductId",
                "ProductName",
                review.ProductId);

            return View(review);
        }

        // GET: Review/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var review = await _context.Reviews.FindAsync(id);
                if (review == null)
                {
                    return NotFound();
                }

                ViewData["CustomerId"] = new SelectList(
                    await _context.Customers.OrderBy(c => c.CustomerName).ToListAsync(),
                    "CustomerId",
                    "CustomerName",
                    review.CustomerId);

                ViewData["ProductId"] = new SelectList(
                    await _context.Products.OrderBy(p => p.ProductName).ToListAsync(),
                    "ProductId",
                    "ProductName",
                    review.ProductId);

                return View(review);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading review for edit");
                TempData["Error"] = "Có l?i khi t?i ?ánh giá";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Review/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReviewId,CustomerId,ProductId,Rating,Comment,CreatedDate")] Review review)
        {
            if (id != review.ReviewId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    review.ModifiedDate = DateTime.Now;
                    _context.Update(review);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "C?p nh?t ?ánh giá thành công";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    _logger.LogError(ex, "Concurrency error editing review");
                    if (!ReviewExists(review.ReviewId))
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
                    _logger.LogError(ex, "Error editing review");
                    ModelState.AddModelError("", "Có l?i khi c?p nh?t ?ánh giá");
                }
            }

            ViewData["CustomerId"] = new SelectList(
                await _context.Customers.OrderBy(c => c.CustomerName).ToListAsync(),
                "CustomerId",
                "CustomerName",
                review.CustomerId);

            ViewData["ProductId"] = new SelectList(
                await _context.Products.OrderBy(p => p.ProductName).ToListAsync(),
                "ProductId",
                "ProductName",
                review.ProductId);

            return View(review);
        }

        // GET: Review/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var review = await _context.Reviews
                    .Include(r => r.Customer)
                    .Include(r => r.Product)
                    .FirstOrDefaultAsync(m => m.ReviewId == id);

                if (review == null)
                {
                    return NotFound();
                }

                return View(review);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading review for delete");
                TempData["Error"] = "Có l?i khi t?i ?ánh giá";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Review/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var review = await _context.Reviews.FindAsync(id);
                if (review != null)
                {
                    _context.Reviews.Remove(review);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Xóa ?ánh giá thành công";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting review");
                TempData["Error"] = "Có l?i khi xóa ?ánh giá";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ReviewExists(int id)
        {
            return _context.Reviews.Any(e => e.ReviewId == id);
        }
    }
}
