using Microsoft.EntityFrameworkCore;
using GarageHub.Domain.Entities;

namespace GarageHub.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Purchase> Purchases { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Customer - User relationship (1:1)
            modelBuilder.Entity<Customer>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Customer - Vehicle relationship (1:many)
            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.Customer)
                .WithMany(c => c.Vehicles)
                .HasForeignKey(v => v.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Customer - Purchase relationship (1:many)
            modelBuilder.Entity<Purchase>()
                .HasOne(p => p.Customer)
                .WithMany(c => c.Purchases)
                .HasForeignKey(p => p.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes for search performance (Feature 10)
            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.FullName)
                .HasDatabaseName("IX_Customers_FullName");

            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.Phone)
                .HasDatabaseName("IX_Customers_Phone");

            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.Email)
                .HasDatabaseName("IX_Customers_Email");

            modelBuilder.Entity<Vehicle>()
                .HasIndex(v => v.VehicleNumber)
                .HasDatabaseName("IX_Vehicles_VehicleNumber");
        }
    }
}