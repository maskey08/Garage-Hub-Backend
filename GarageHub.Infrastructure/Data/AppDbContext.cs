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

        // Configure User and relationships
        modelBuilder.Entity<User>().Property(u => u.FirstName).HasMaxLength(100);
        modelBuilder.Entity<User>().Property(u => u.LastName).HasMaxLength(100);
        modelBuilder.Entity<User>().Property(u => u.Phone).HasMaxLength(20);

        // Vehicle → User
        modelBuilder.Entity<Vehicle>(e => {
            e.HasKey(v => v.VehicleId);
            e.HasOne(v => v.User)
             .WithMany(u => u.Vehicles)
             .HasForeignKey(v => v.UserId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // Appointment → Customer (User) and Vehicle
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

        // Review → Appointment and Customer
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

        // SalesInvoiceItem → SalesInvoice and Part
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
        });

        // Sale → Customer
        modelBuilder.Entity<Sale>(e => {
            e.HasKey(s => s.Id);
            e.HasOne<User>()
             .WithMany()
             .HasForeignKey(s => s.CustomerId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // SaleItem → Sale and Part
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
        });

        // Invoice → Sale
        modelBuilder.Entity<Invoice>(e => {
            e.HasKey(i => i.Id);
            e.HasOne(i => i.Sale)
             .WithMany()
             .HasForeignKey(i => i.SaleId)
             .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
