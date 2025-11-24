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

            // Configure Customer relationships
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

            // Configure column types
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

            // Add seed data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Products
            modelBuilder.Entity<Product>().HasData(
                new Product { ProductId = 1, ProductName = "Áo Thun Nam", Description = "Áo thun nam ch?t l??ng cao", Price = 150000, Stock = 100, Category = "Áo", CreatedDate = DateTime.Now },
                new Product { ProductId = 2, ProductName = "Áo S? Mi Nam", Description = "Áo s? mi nam ki?u dáng hi?n ??i", Price = 250000, Stock = 50, Category = "Áo", CreatedDate = DateTime.Now },
                new Product { ProductId = 3, ProductName = "Qu?n Jeans Nam", Description = "Qu?n jeans nam b?n b?", Price = 350000, Stock = 75, Category = "Qu?n", CreatedDate = DateTime.Now },
                new Product { ProductId = 4, ProductName = "Áo Dài N?", Description = "Áo dài truy?n th?ng sang tr?ng", Price = 450000, Stock = 40, Category = "Áo", CreatedDate = DateTime.Now },
                new Product { ProductId = 5, ProductName = "Váy N?", Description = "Váy n? th?i trang", Price = 300000, Stock = 60, Category = "Váy", CreatedDate = DateTime.Now }
            );

            // Seed Customers
            modelBuilder.Entity<Customer>().HasData(
                new Customer { CustomerId = 1, CustomerName = "Nguy?n V?n A", Phone = "0912345678", Email = "nguyenvana@example.com", Address = "123 ???ng 1, TP.HCM", City = "TP.HCM", PostalCode = "70000", CreatedDate = DateTime.Now },
                new Customer { CustomerId = 2, CustomerName = "Tr?n Th? B", Phone = "0987654321", Email = "tranthib@example.com", Address = "456 ???ng 2, HN", City = "Hà N?i", PostalCode = "10000", CreatedDate = DateTime.Now },
                new Customer { CustomerId = 3, CustomerName = "Ph?m V?n C", Phone = "0901234567", Email = "phamvanc@example.com", Address = "789 ???ng 3, ?N", City = "?à N?ng", PostalCode = "50000", CreatedDate = DateTime.Now }
            );
        }
    }
}
