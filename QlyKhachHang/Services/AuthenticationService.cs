using System.Security.Cryptography;
using System.Text;
using QlyKhachHang.Data;
using QlyKhachHang.Models;
using Microsoft.EntityFrameworkCore;

namespace QlyKhachHang.Services
{
    public interface IAuthenticationService
    {
        Task<Customer?> LoginAsync(string usernameOrEmail, string password);
        Task<(bool Success, string Message)> RegisterAsync(RegisterViewModel model);
        Task<Customer?> GetCustomerByUsernameAsync(string username);
        Task<Customer?> GetCustomerByEmailAsync(string email);
        Task<bool> UpdateLastLoginAsync(int customerId);
        string HashPassword(string password);
        bool VerifyPassword(string password, string hash);
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AuthenticationService> _logger;

        public AuthenticationService(ApplicationDbContext context, ILogger<AuthenticationService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Customer?> LoginAsync(string usernameOrEmail, string password)
        {
            try
            {
                var customer = await _context.Customers
                    .FirstOrDefaultAsync(c => 
                        (c.Username == usernameOrEmail || c.Email == usernameOrEmail) &&
                        c.Status == "Active");

                if (customer == null)
                {
                    _logger.LogWarning($"Login attempt failed: user not found for {usernameOrEmail}");
                    return null;
                }

                if (!VerifyPassword(password, customer.PasswordHash))
                {
                    _logger.LogWarning($"Login attempt failed: incorrect password for {usernameOrEmail}");
                    return null;
                }

                return customer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login process");
                return null;
            }
        }

        public async Task<(bool Success, string Message)> RegisterAsync(RegisterViewModel model)
        {
            try
            {
                // Kiểm tra username đã tồn tại
                if (await _context.Customers.AnyAsync(c => c.Username == model.Username))
                {
                    return (false, "Tên đăng nhập đã tồn tại");
                }

                // Kiểm tra email đã tồn tại
                if (await _context.Customers.AnyAsync(c => c.Email == model.Email))
                {
                    return (false, "Email đã được sử dụng");
                }

                var customer = new Customer
                {
                    CustomerName = model.CustomerName,
                    Username = model.Username,
                    Email = model.Email,
                    Phone = model.Phone,
                    Address = model.Address,
                    City = model.City,
                    PostalCode = model.PostalCode,
                    PasswordHash = HashPassword(model.Password),
                    Status = "Active",
                    CreatedDate = DateTime.Now
                };

                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"New customer registered: {model.Username}");
                return (true, "Đăng ký thành công");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration");
                return (false, "Có lỗi khi đăng ký");
            }
        }

        public async Task<Customer?> GetCustomerByUsernameAsync(string username)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(c => c.Username == username && c.Status == "Active");
        }

        public async Task<Customer?> GetCustomerByEmailAsync(string email)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(c => c.Email == email && c.Status == "Active");
        }

        public async Task<bool> UpdateLastLoginAsync(int customerId)
        {
            try
            {
                var customer = await _context.Customers.FindAsync(customerId);
                if (customer != null)
                {
                    customer.LastLoginDate = DateTime.Now;
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating last login");
                return false;
            }
        }

        public string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        public bool VerifyPassword(string password, string hash)
        {
            var hashOfInput = HashPassword(password);
            return hashOfInput == hash;
        }
    }
}
