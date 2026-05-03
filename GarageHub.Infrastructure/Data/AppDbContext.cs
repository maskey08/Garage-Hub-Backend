using GarageHub.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GarageHub.Infrastructure.Data;

public class AppDbContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<PartRequest> PartRequests => Set<PartRequest>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<SalesInvoice> SalesInvoices => Set<SalesInvoice>();
    public DbSet<SalesInvoiceItem> SalesInvoiceItems => Set<SalesInvoiceItem>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<Part> Parts { get; set; }

    public DbSet<Sale> Sales { get; set; }

    public DbSet<SaleItem> SaleItems { get; set; }

    public DbSet<Invoice> Invoices { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Vehicle → User
        modelBuilder.Entity<Vehicle>(e => {
            e.HasKey(v => v.VehicleId);
            e.HasOne(v => v.User)
             .WithMany(u => u.Vehicles)
             .HasForeignKey(v => v.UserId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // Appointment → Customer (User)
        modelBuilder.Entity<Appointment>(e => {
            e.HasKey(a => a.AppointmentId);
            e.HasOne(a => a.Customer)
             .WithMany(u => u.Appointments)
             .HasForeignKey(a => a.CustomerId)
             .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(a => a.Vehicle)
             .WithMany(v => v.Appointments)
             .HasForeignKey(a => a.VehicleId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // Review → Appointment (one-to-one)
        modelBuilder.Entity<Review>(e => {
            e.HasKey(r => r.ReviewId);
            e.HasOne(r => r.Appointment)
             .WithOne(a => a.Review)
             .HasForeignKey<Review>(r => r.AppointmentId)
             .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(r => r.Customer)
             .WithMany(u => u.Reviews)
             .HasForeignKey(r => r.CustomerId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // PartRequest → Customer
        modelBuilder.Entity<PartRequest>(e => {
            e.HasKey(p => p.RequestId);
            e.HasOne(p => p.Customer)
             .WithMany(u => u.PartRequests)
             .HasForeignKey(p => p.CustomerId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // SalesInvoice → Customer
        modelBuilder.Entity<SalesInvoice>(e => {
            e.HasKey(s => s.SaleId);
            e.HasOne(s => s.Customer)
             .WithMany(u => u.SalesInvoices)
             .HasForeignKey(s => s.CustomerId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // SalesInvoiceItem → SalesInvoice
        modelBuilder.Entity<SalesInvoiceItem>(e => {
            e.HasKey(i => i.ItemId);
            e.HasOne(i => i.SalesInvoice)
             .WithMany(s => s.Items)
             .HasForeignKey(i => i.SaleId)
             .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(i => i.Part)
             .WithMany()
             .HasForeignKey(i => i.PartId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // Notification → User
        modelBuilder.Entity<Notification>(e => {
            e.HasKey(n => n.NotificationId);
            e.HasOne(n => n.User)
             .WithMany(u => u.Notifications)
             .HasForeignKey(n => n.UserId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // Part
        modelBuilder.Entity<Part>(e => {
            e.HasKey(p => p.Id);
            e.Property(p => p.PartName).IsRequired();
            e.Property(p => p.PartNumber).IsRequired();
            e.Property(p => p.Category).IsRequired();
            e.Property(p => p.Brand).IsRequired();
            e.Property(p => p.Price).HasPrecision(10, 2);
            e.Property(p => p.LowStockThreshold).HasDefaultValue(0);
        });

        // Sale → Customer (User)
        modelBuilder.Entity<Sale>(e => {
            e.HasKey(s => s.Id);
            e.HasOne<User>()
             .WithMany()
             .HasForeignKey(s => s.CustomerId)
             .OnDelete(DeleteBehavior.Restrict);
            e.Property(s => s.SubTotal).HasPrecision(10, 2);
            e.Property(s => s.TaxAmount).HasPrecision(10, 2);
            e.Property(s => s.GrandTotal).HasPrecision(10, 2);
        });

        // SaleItem → Sale, Part
        modelBuilder.Entity<SaleItem>(e => {
            e.HasKey(si => si.Id);
            e.HasOne(si => si.Sale)
             .WithMany(s => s.SaleItems)
             .HasForeignKey(si => si.SaleId)
             .OnDelete(DeleteBehavior.Cascade);
            e.HasOne<Part>()
             .WithMany()
             .HasForeignKey(si => si.PartId)
             .OnDelete(DeleteBehavior.Restrict);
            e.Property(si => si.UnitPrice).HasPrecision(10, 2);
            e.Property(si => si.TotalPrice).HasPrecision(10, 2);
        });

        // Invoice → Sale
        modelBuilder.Entity<Invoice>(e => {
            e.HasKey(i => i.Id);
            e.HasOne(i => i.Sale)
             .WithMany()
             .HasForeignKey(i => i.SaleId)
             .OnDelete(DeleteBehavior.Cascade);
            e.Property(i => i.InvoiceNumber).IsRequired();
        });
    }
}
