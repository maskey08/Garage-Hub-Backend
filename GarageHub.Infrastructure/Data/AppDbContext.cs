using GarageHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GarageHub.Infrastructure.Data;

public class AppDbContext : DbContext
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



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // User
        modelBuilder.Entity<User>(e => {
            e.ToTable("users");
            e.HasKey(u => u.UserId);
            e.Property(u => u.UserId).HasColumnName("user_id");
            e.Property(u => u.Email).HasColumnName("email");
            e.Property(u => u.Phone).HasColumnName("phone");
            e.Property(u => u.PasswordHash).HasColumnName("password_hash");
            e.Property(u => u.Role).HasColumnName("role").HasDefaultValue("customer");
            e.Property(u => u.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'").ValueGeneratedOnAdd()
            .HasConversion(
                v => v,
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
            ); 
            e.Property(u => u.LoyaltyPoints).HasColumnName("LoyaltyPoints");
        });

        // Vehicle → User
        modelBuilder.Entity<Vehicle>(e => {
            e.ToTable("vehicles");
            e.HasKey(v => v.VehicleId);
            e.HasOne(v => v.User)
             .WithMany(u => u.Vehicles)
             .HasForeignKey(v => v.UserId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // Appointment → Customer (User)
        modelBuilder.Entity<Appointment>(e => {
            e.ToTable("appointments");
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
            e.ToTable("reviews");
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
            e.ToTable("part_requests");
            e.HasKey(p => p.RequestId);
            e.HasOne(p => p.Customer)
             .WithMany(u => u.PartRequests)
             .HasForeignKey(p => p.CustomerId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // SalesInvoice → Customer
        modelBuilder.Entity<SalesInvoice>(e => {
            e.ToTable("sales_invoices");
            e.HasKey(s => s.SaleId);
            e.HasOne(s => s.Customer)
             .WithMany(u => u.SalesInvoices)
             .HasForeignKey(s => s.CustomerId)
             .OnDelete(DeleteBehavior.Restrict);

            e.Property(s => s.SaleId).HasColumnName("sale_id");
            e.Property(s => s.CustomerId).HasColumnName("customer_id");
            e.Property(s => s.StaffId).HasColumnName("staff_id");
            e.Property(s => s.SaleDate).HasColumnName("sale_date");

            e.Property(s => s.Subtotal).HasColumnName("subtotal");
            e.Property(s => s.TotalAmount).HasColumnName("total_amount");
            e.Property(s => s.CreditUsed).HasColumnName("credit_used");
            e.Property(s => s.PaymentStatus).HasColumnName("payment_status");
            e.Property(s => s.DiscountApplied).HasColumnName("discount_applied");
        });

        // SalesInvoiceItem → SalesInvoice
        modelBuilder.Entity<SalesInvoiceItem>(e => {
            e.ToTable("sales_invoice_items");
            e.HasKey(i => i.ItemId);
            e.HasOne(i => i.SalesInvoice)
             .WithMany(s => s.Items)
             .HasForeignKey(i => i.SaleId)
             .OnDelete(DeleteBehavior.Cascade);

            e.Property(i => i.ItemId).HasColumnName("item_id");
            e.Property(i => i.SaleId).HasColumnName("sale_id");

            e.Property(i => i.PartId).HasColumnName("part_id");

            e.Property(i => i.Quantity).HasColumnName("quantity");
            e.Property(i => i.UnitPrice).HasColumnName("unit_price");
        });

        // Notification → User
        modelBuilder.Entity<Notification>(e => {
            e.ToTable("notifications");
            e.HasKey(n => n.NotificationId);
            e.Property(n => n.SentAt)
             .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'")
             .ValueGeneratedOnAdd();
            e.HasOne(n => n.User)
             .WithMany(u => u.Notifications)
             .HasForeignKey(n => n.UserId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // Review → Appointment (one-to-one)
        modelBuilder.Entity<Review>(e => {
            e.Property(r => r.ReviewedAt)
             .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'")
             .ValueGeneratedOnAdd();
        });

        // PartRequest → Customer
        modelBuilder.Entity<PartRequest>(e => {
            e.Property(p => p.RequestedAt)
             .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'")
             .ValueGeneratedOnAdd();
        });

        // SalesInvoice → Customer
        modelBuilder.Entity<SalesInvoice>(e => {
            e.Property(s => s.SaleDate)
             .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'")
             .ValueGeneratedOnAdd();
        });

        // Sale
        modelBuilder.Entity<Sale>(e => {
            e.ToTable("sales");
            e.HasKey(s => s.Id);
            e.Property(s => s.SaleDate)
             .HasColumnName("sale_date")
             .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'")
             .ValueGeneratedOnAdd();
        });

        

        //parts
        modelBuilder.Entity<Part>(e => {
            e.ToTable("parts");
            e.HasKey(p => p.PartId);
            e.Property(p => p.PartId).HasColumnName("part_id");
            e.Property(p => p.PartName).HasColumnName("part_name");
            e.Property(p => p.Brand).HasColumnName("brand");
            e.Property(p => p.Price).HasColumnName("unit_price");
            e.Property(p => p.StockQuantity).HasColumnName("stock_qty");
        });
    }
}
