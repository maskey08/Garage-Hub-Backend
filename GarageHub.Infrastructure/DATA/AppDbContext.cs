using GarageHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GarageHub.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Part> Parts { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<PurchaseInvoice> PurchaseInvoices { get; set; }
        public DbSet<PurchaseInvoiceItem> PurchaseInvoiceItems { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Part>()
                .HasOne(p => p.Vendor)
                .WithMany(v => v.Parts)
                .HasForeignKey(p => p.VendorId);

            modelBuilder.Entity<PurchaseInvoice>()
                .HasOne(i => i.Vendor)
                .WithMany(v => v.PurchaseInvoices)
                .HasForeignKey(i => i.VendorId);

            modelBuilder.Entity<PurchaseInvoiceItem>()
                .HasOne(i => i.PurchaseInvoice)
                .WithMany(pi => pi.Items)
                .HasForeignKey(i => i.PurchaseInvoiceId);

            modelBuilder.Entity<PurchaseInvoiceItem>()
                .HasOne(i => i.Part)
                .WithMany()
                .HasForeignKey(i => i.PartId);

            modelBuilder.Entity<PurchaseInvoiceItem>()
                .Ignore(i => i.TotalPrice);
        }
    }
}