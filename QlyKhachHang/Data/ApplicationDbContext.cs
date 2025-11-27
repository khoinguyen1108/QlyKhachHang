using Microsoft.EntityFrameworkCore;
using QlyKhachHang.Models;

namespace QlyKhachHang.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; } = null!; 
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Cart> Carts { get; set; } = null!;
        public DbSet<CartItem> CartItems { get; set; } = null!;
        public DbSet<Review> Reviews { get; set; } = null!;
        public DbSet<Invoice> Invoices { get; set; } = null!;
        public DbSet<InvoiceDetail> InvoiceDetails { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User relationships
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            // Configure Customer relationships and constraints
            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.Email)
                .IsUnique();

            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.Username)
                .IsUnique();

            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Carts)
                .WithOne(c => c.Customer)
                .HasForeignKey(c => c.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Reviews)
                .WithOne(r => r.Customer)
                .HasForeignKey(r => r.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Invoices)
                .WithOne(i => i.Customer)
                .HasForeignKey(i => i.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Product relationships
            modelBuilder.Entity<Product>()
                .HasMany(p => p.CartItems)
                .WithOne(ci => ci.Product)
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.Reviews)
                .WithOne(r => r.Product)
                .HasForeignKey(r => r.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.InvoiceDetails)
                .WithOne(id => id.Product)
                .HasForeignKey(id => id.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Cart relationships
            modelBuilder.Entity<Cart>()
                .HasMany(c => c.CartItems)
                .WithOne(ci => ci.Cart)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Invoice relationships
            modelBuilder.Entity<Invoice>()
                .HasMany(i => i.InvoiceDetails)
                .WithOne(id => id.Invoice)
                .HasForeignKey(id => id.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure column types and precision
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(10, 2);

            modelBuilder.Entity<CartItem>()
                .Property(ci => ci.UnitPrice)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Invoice>()
                .Property(i => i.TotalAmount)
                .HasPrecision(12, 2);

            modelBuilder.Entity<InvoiceDetail>()
                .Property(id => id.UnitPrice)
                .HasPrecision(10, 2);

            // Seed data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User { UserID = 1, Name = "Admin", Username = "admin", Email = "admin@shop.com", Password = "123456", Role = "Admin", CreatedAt = new DateTime(2025, 11, 24, 14, 10, 19) },
                new User { UserID = 2, Name = "Nhân Viên", Username = "nhanvien", Email = "staff@shop.com", Password = "123456", Role = "Employee", CreatedAt = new DateTime(2025, 11, 24, 14, 10, 19) },
                new User { UserID = 3, Name = "Khách Hàng 1", Username = "khachhang1", Email = "kh1@gmail.com", Password = "MatKhauMoi_123", Role = "Customer", CreatedAt = new DateTime(2025, 11, 24, 14, 10, 19) },
                new User { UserID = 4, Name = "Khách Hàng 2", Username = "khachhang2", Email = "kh2@gmail.com", Password = "123456", Role = "Customer", CreatedAt = new DateTime(2025, 11, 24, 14, 10, 19) }
            );

            // Seed Products
            modelBuilder.Entity<Product>().HasData(
                new Product { ProductId = 1, ProductName = "Áo Nike Air", Description = "Áo thể thao Nike Air chất lượng cao", Price = 500000m, Stock = 100, Category = "Áo", CreatedDate = new DateTime(2025, 11, 24, 14, 10, 19) },
                new Product { ProductId = 2, ProductName = "Áo Adidas Sport", Description = "Áo thể thao Adidas Sport", Price = 450000m, Stock = 150, Category = "Áo", CreatedDate = new DateTime(2025, 11, 24, 14, 10, 19) },
                new Product { ProductId = 3, ProductName = "Áo Puma Run", Description = "Áo chạy bộ Puma Run", Price = 480000m, Stock = 120, Category = "Áo", CreatedDate = new DateTime(2025, 11, 24, 14, 10, 19) },
                new Product { ProductId = 4, ProductName = "Áo Reebok Classic", Description = "Áo cổ điển Reebok Classic", Price = 420000m, Stock = 80, Category = "Áo", CreatedDate = new DateTime(2025, 11, 24, 14, 10, 19) },
                new Product { ProductId = 5, ProductName = "Áo New Balance Pro", Description = "Áo chuyên nghiệp New Balance Pro", Price = 520000m, Stock = 95, Category = "Áo", CreatedDate = new DateTime(2025, 11, 24, 14, 10, 19) },
                new Product { ProductId = 6, ProductName = "Quần Nike Flex", Description = "Quần Nike Flex thoáng khí", Price = 350000m, Stock = 110, Category = "Quần", CreatedDate = new DateTime(2025, 11, 24, 14, 10, 19) },
                new Product { ProductId = 7, ProductName = "Quần Adidas Training", Description = "Quần tập luyện Adidas Training", Price = 380000m, Stock = 100, Category = "Quần", CreatedDate = new DateTime(2025, 11, 24, 14, 10, 19) },
                new Product { ProductId = 8, ProductName = "Quần Puma Comfort", Description = "Quần thoải mái Puma Comfort", Price = 360000m, Stock = 130, Category = "Quần", CreatedDate = new DateTime(2025, 11, 24, 14, 10, 19) },
                new Product { ProductId = 9, ProductName = "Quần Reebok Sport", Description = "Quần thể thao Reebok Sport", Price = 340000m, Stock = 105, Category = "Quần", CreatedDate = new DateTime(2025, 11, 24, 14, 10, 19) },
                new Product { ProductId = 10, ProductName = "Quần New Balance Active", Description = "Quần năng động New Balance Active", Price = 390000m, Stock = 85, Category = "Quần", CreatedDate = new DateTime(2025, 11, 24, 14, 10, 19) },
                new Product { ProductId = 11, ProductName = "Giày Nike Runner", Description = "Giày chạy Nike Runner", Price = 750000m, Stock = 60, Category = "Giày", CreatedDate = new DateTime(2025, 11, 24, 14, 10, 19) },
                new Product { ProductId = 12, ProductName = "Giày Adidas Ultra", Description = "Giày cao cấp Adidas Ultra", Price = 800000m, Stock = 50, Category = "Giày", CreatedDate = new DateTime(2025, 11, 24, 14, 10, 19) },
                new Product { ProductId = 13, ProductName = "Giày Puma Velocity", Description = "Giày tốc độ Puma Velocity", Price = 720000m, Stock = 70, Category = "Giày", CreatedDate = new DateTime(2025, 11, 24, 14, 10, 19) },
                new Product { ProductId = 14, ProductName = "Giày Reebok Storm", Description = "Giày bão Reebok Storm", Price = 680000m, Stock = 75, Category = "Giày", CreatedDate = new DateTime(2025, 11, 24, 14, 10, 19) },
                new Product { ProductId = 15, ProductName = "Giày New Balance Fresh", Description = "Giày tươi mới New Balance Fresh", Price = 820000m, Stock = 55, Category = "Giày", CreatedDate = new DateTime(2025, 11, 24, 14, 10, 19) },
                new Product { ProductId = 16, ProductName = "Mũ Nike Classic", Description = "Mũ cổ điển Nike Classic", Price = 150000m, Stock = 200, Category = "Mũ", CreatedDate = new DateTime(2025, 11, 24, 14, 10, 19) },
                new Product { ProductId = 17, ProductName = "Mũ Adidas Sport", Description = "Mũ thể thao Adidas Sport", Price = 160000m, Stock = 180, Category = "Mũ", CreatedDate = new DateTime(2025, 11, 24, 14, 10, 19) },
                new Product { ProductId = 18, ProductName = "Mũ Puma Pro", Description = "Mũ chuyên nghiệp Puma Pro", Price = 155000m, Stock = 190, Category = "Mũ", CreatedDate = new DateTime(2025, 11, 24, 14, 10, 19) },
                new Product { ProductId = 19, ProductName = "Mũ Reebok Active", Description = "Mũ năng động Reebok Active", Price = 145000m, Stock = 210, Category = "Mũ", CreatedDate = new DateTime(2025, 11, 24, 14, 10, 19) },
                new Product { ProductId = 20, ProductName = "Mũ New Balance Fashion", Description = "Mũ thời trang New Balance Fashion", Price = 165000m, Stock = 170, Category = "Mũ", CreatedDate = new DateTime(2025, 11, 24, 14, 10, 19) },
                new Product { ProductId = 21, ProductName = "Tất Nike Comfort", Description = "Tất thoải mái Nike Comfort", Price = 80000m, Stock = 300, Category = "Tất", CreatedDate = new DateTime(2025, 11, 24, 14, 10, 19) },
                new Product { ProductId = 22, ProductName = "Tất Adidas Premium", Description = "Tất cao cấp Adidas Premium", Price = 90000m, Stock = 280, Category = "Tất", CreatedDate = new DateTime(2025, 11, 24, 14, 10, 19) },
                new Product { ProductId = 23, ProductName = "Tất Puma Elite", Description = "Tất tinh hoa Puma Elite", Price = 85000m, Stock = 290, Category = "Tất", CreatedDate = new DateTime(2025, 11, 24, 14, 10, 19) },
                new Product { ProductId = 24, ProductName = "Tất Reebok Sport", Description = "Tất thể thao Reebok Sport", Price = 75000m, Stock = 310, Category = "Tất", CreatedDate = new DateTime(2025, 11, 24, 14, 10, 19) },
                new Product { ProductId = 25, ProductName = "Tất New Balance Tech", Description = "Tất công nghệ New Balance Tech", Price = 95000m, Stock = 270, Category = "Tất", CreatedDate = new DateTime(2025, 11, 24, 14, 10, 19) },
                new Product { ProductId = 26, ProductName = "Áo Khoác Nike Wind", Description = "Áo khoác gió Nike Wind", Price = 650000m, Stock = 75, Category = "Áo Khoác", CreatedDate = new DateTime(2025, 11, 24, 14, 10, 19) },
                new Product { ProductId = 27, ProductName = "Áo Khoác Adidas Boost", Description = "Áo khoác tăng cường Adidas Boost", Price = 700000m, Stock = 65, Category = "Áo Khoác", CreatedDate = new DateTime(2025, 11, 24, 14, 10, 19) },
                new Product { ProductId = 28, ProductName = "Áo Khoác Puma Fierce", Description = "Áo khoác mãnh liệt Puma Fierce", Price = 620000m, Stock = 80, Category = "Áo Khoác", CreatedDate = new DateTime(2025, 11, 24, 14, 10, 19) },
                new Product { ProductId = 29, ProductName = "Áo Khoác Reebok Storm", Description = "Áo khoác bão Reebok Storm", Price = 580000m, Stock = 90, Category = "Áo Khoác", CreatedDate = new DateTime(2025, 11, 24, 14, 10, 19) },
                new Product { ProductId = 30, ProductName = "Áo Khoác New Balance Heat", Description = "Áo khoác nhiệt New Balance Heat", Price = 720000m, Stock = 70, Category = "Áo Khoác", CreatedDate = new DateTime(2025, 11, 24, 14, 10, 19) },
                new Product { ProductId = 31, ProductName = "Thắt Lưng Nike Pro", Description = "Thắt lưng chuyên nghiệp Nike Pro", Price = 120000m, Stock = 220, Category = "Thắt Lưng", CreatedDate = new DateTime(2025, 11, 24, 14, 10, 19) },
                new Product { ProductId = 32, ProductName = "Thắt Lưng Adidas Classic", Description = "Thắt lưng cổ điển Adidas Classic", Price = 130000m, Stock = 200, Category = "Thắt Lưng", CreatedDate = new DateTime(2025, 11, 24, 14, 10, 19) },
                new Product { ProductId = 33, ProductName = "Thắt Lưng Puma Active", Description = "Thắt lưng năng động Puma Active", Price = 125000m, Stock = 210, Category = "Thắt Lưng", CreatedDate = new DateTime(2025, 11, 24, 14, 10, 19) },
                new Product { ProductId = 34, ProductName = "Thắt Lưng Reebok Sport", Description = "Thắt lưng thể thao Reebok Sport", Price = 115000m, Stock = 230, Category = "Thắt Lưng", CreatedDate = new DateTime(2025, 11, 24, 14, 10, 19) },
                new Product { ProductId = 35, ProductName = "Thắt Lưng New Balance", Description = "Thắt lưng New Balance", Price = 135000m, Stock = 190, Category = "Thắt Lưng", CreatedDate = new DateTime(2025, 11, 24, 14, 10, 19) },
                new Product { ProductId = 36, ProductName = "Ba Lô Nike Travel", Description = "Ba lô du lịch Nike Travel", Price = 550000m, Stock = 55, Category = "Ba Lô", CreatedDate = new DateTime(2025, 11, 24, 14, 10, 19) },
                new Product { ProductId = 37, ProductName = "Ba Lô Adidas Urban", Description = "Ba lô đô thị Adidas Urban", Price = 600000m, Stock = 45, Category = "Ba Lô", CreatedDate = new DateTime(2025, 11, 24, 14, 10, 19) },
                new Product { ProductId = 38, ProductName = "Ba Lô Puma School", Description = "Ba lô học sinh Puma School", Price = 520000m, Stock = 65, Category = "Ba Lô", CreatedDate = new DateTime(2025, 11, 24, 14, 10, 19) },
                new Product { ProductId = 39, ProductName = "Ba Lô Reebok Explorer", Description = "Ba lô khám phá Reebok Explorer", Price = 480000m, Stock = 75, Category = "Ba Lô", CreatedDate = new DateTime(2025, 11, 24, 14, 10, 19) },
                new Product { ProductId = 40, ProductName = "Ba Lô New Balance Trek", Description = "Ba lô trekking New Balance Trek", Price = 620000m, Stock = 50, Category = "Ba Lô", CreatedDate = new DateTime(2025, 11, 24, 14, 10, 19) },
                new Product { ProductId = 41, ProductName = "Dây Giày Nike Pro", Description = "Dây giày chuyên nghiệp Nike Pro", Price = 50000m, Stock = 400, Category = "Dây Giày", CreatedDate = new DateTime(2025, 11, 24, 14, 10, 19) },
                new Product { ProductId = 42, ProductName = "Dây Giày Adidas Sport", Description = "Dây giày thể thao Adidas Sport", Price = 55000m, Stock = 380, Category = "Dây Giày", CreatedDate = new DateTime(2025, 11, 24, 14, 10, 19) },
                new Product { ProductId = 43, ProductName = "Dây Giày Puma Strong", Description = "Dây giày chắc chắn Puma Strong", Price = 52000m, Stock = 390, Category = "Dây Giày", CreatedDate = new DateTime(2025, 11, 24, 14, 10, 19) },
                new Product { ProductId = 44, ProductName = "Dây Giày Reebok Flex", Description = "Dây giày linh hoạt Reebok Flex", Price = 48000m, Stock = 410, Category = "Dây Giày", CreatedDate = new DateTime(2025, 11, 24, 14, 10, 19) },
                new Product { ProductId = 45, ProductName = "Dây Giày New Balance Tech", Description = "Dây giày công nghệ New Balance Tech", Price = 58000m, Stock = 370, Category = "Dây Giày", CreatedDate = new DateTime(2025, 11, 24, 14, 10, 19) },
                new Product { ProductId = 46, ProductName = "Túi Đeo Nike Sprint", Description = "Túi đeo nhanh Nike Sprint", Price = 250000m, Stock = 120, Category = "Túi", CreatedDate = new DateTime(2025, 11, 24, 14, 10, 19) },
                new Product { ProductId = 47, ProductName = "Túi Đeo Adidas Move", Description = "Túi đeo di chuyển Adidas Move", Price = 280000m, Stock = 100, Category = "Túi", CreatedDate = new DateTime(2025, 11, 24, 14, 10, 19) },
                new Product { ProductId = 48, ProductName = "Túi Đeo Puma Quick", Description = "Túi đeo nhanh chóng Puma Quick", Price = 260000m, Stock = 110, Category = "Túi", CreatedDate = new DateTime(2025, 11, 24, 14, 10, 19) },
                new Product { ProductId = 49, ProductName = "Túi Đeo Reebok Fast", Description = "Túi đeo nhanh Reebok Fast", Price = 240000m, Stock = 130, Category = "Túi", CreatedDate = new DateTime(2025, 11, 24, 14, 10, 19) },
                new Product { ProductId = 50, ProductName = "Túi Đeo New Balance Connect", Description = "Túi đeo kết nối New Balance Connect", Price = 290000m, Stock = 105, Category = "Túi", CreatedDate = new DateTime(2025, 11, 24, 14, 10, 19) }
            );
           

            // Customer seeding moved to AddProperCustomerSeeding migration
            // This ensures password hashes are properly generated
        }
    }
}
