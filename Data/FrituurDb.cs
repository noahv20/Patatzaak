using Microsoft.EntityFrameworkCore;
using Patatzaak.Models;

namespace Patatzaak.Data
{
    public class FrituurDb : DbContext 
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionstring = "Data Source=.;Initial Catalog=FrituurDb;Integrated Security=True; TrustServerCertificate=true;";
            optionsBuilder.UseSqlServer(connectionstring);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Specify constraints
            // Products

            modelBuilder.Entity<Product>()
                .Property(p => p.Name)
                .HasMaxLength(30);

            modelBuilder.Entity<Product>()
                .Property(p => p.Description)
                .HasMaxLength(150);

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasConversion<decimal>()
                .HasColumnType("Decimal(18,2)");

            modelBuilder.Entity<Product>()
                .Property(p => p.Sale)
                .HasConversion<int>();

            modelBuilder.Entity<Product>()
                .Property(p => p.Stock)
                .HasConversion<int>();

            Product p = new Product()
            {
                Id = 1,
                Name = "Friet",
                Description = "Gefrituurde aardappelen",
                Price = 2.0m,
                Sale = 0,
                Stock = 20
            };

            modelBuilder.Entity<Product>()
                .HasData(p);

            // Order

            modelBuilder.Entity<Order>()
                .Property(o => o.State)
                .HasMaxLength(30);

            modelBuilder.Entity<Order>()
                .Property(o => o.OrderedOn)
                .HasConversion<DateTime>();

            Order o = new Order()
            {
                OrderNr = 1,
                OrderedOn = DateTime.Now,
                State = "In Progress",
                CustomerId = 1
            };

            modelBuilder.Entity<Order>()
                .HasData(o);

            // OrderItem
            modelBuilder.Entity<OrderItem>()
            .HasKey(oi => new { oi.OrderNr, oi.ProductId });

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderNr);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.ProductId);

            OrderItem oi = new OrderItem()
            {
                OrderNr = 1,
                ProductId = 1,
                Amount = 3
            };

            
            modelBuilder.Entity<OrderItem>()
                .HasData(oi);

            // Customer
            modelBuilder.Entity<Customer>()
                .Property(c => c.Name)
                .HasMaxLength(50);

            modelBuilder.Entity<Customer>()
                .Property(c => c.Email)
                .HasMaxLength(251);

            modelBuilder.Entity<Customer>()
                .Property(c => c.Phone)
                .HasConversion<string>();

            Customer c = new Customer()
            {
                Id = 1,
                Name = "Miel",
                Email = "Miel.Noelanders@Zuyd.nl",
                Phone = null,
                SavedPoints = null
            };

            modelBuilder.Entity<Customer>()
                .HasData(c);

            // Employee

            modelBuilder.Entity<Employee>()
                .Property( e=> e.Name)
                .HasMaxLength(50);

            modelBuilder.Entity<Employee>()
                .Property( e=> e.DateOfEmployment)
                .HasConversion<DateTime>();

            Employee e = new Employee()
            {
                Id = 1,
                Name = "Rob",
                DateOfEmployment = DateTime.Now
            };

            modelBuilder.Entity<Employee>()
                .HasData(e);
        }

    }
}
