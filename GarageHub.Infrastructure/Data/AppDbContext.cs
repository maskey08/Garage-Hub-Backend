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
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Part> Parts { get; set; }
    public DbSet<Sale> Sales { get; set; }
    public DbSet<SaleItem> SaleItems { get; set; }
    public DbSet<Vendor> Vendors { get; set; }
    public DbSet<PurchaseInvoice> PurchaseInvoices { get; set; }
    public DbSet<PurchaseInvoiceItem> PurchaseInvoiceItems { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().ToTable("users");

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var tableName = entityType.GetTableName();
            if (!string.IsNullOrWhiteSpace(tableName))
            {
                entityType.SetTableName(ToSnakeCase(tableName));
            }
        }

        // Configure User and relationships
        modelBuilder.Entity<User>().Property(u => u.FirstName).HasMaxLength(50);
        modelBuilder.Entity<User>().Property(u => u.LastName).HasMaxLength(50);
        modelBuilder.Entity<User>().Property(u => u.Email).HasMaxLength(100);
        modelBuilder.Entity<User>().Property(u => u.Phone).HasMaxLength(20);
        modelBuilder.Entity<User>().Property(u => u.PasswordHashText).HasMaxLength(255);
        modelBuilder.Entity<User>().Property(u => u.Role).HasMaxLength(20);
        modelBuilder.Entity<User>().Property(u => u.TotalSpent).HasColumnType("numeric(12,2)");
        modelBuilder.Entity<User>().Property(u => u.CreditBalance).HasColumnType("numeric(12,2)");
        modelBuilder.Entity<User>().Property(u => u.CreditDueDate).HasColumnType("date");

        // Vehicle → User
        modelBuilder.Entity<Vehicle>(e => {
            e.ToTable("vehicles");
            e.HasKey(v => v.VehicleId);
            e.HasOne(v => v.User)
             .WithMany(u => u.Vehicles)
             .HasForeignKey(v => v.UserId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // Appointment → Customer (User) and Vehicle
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

        // Review → Appointment and Customer
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

        // SalesInvoiceItem → SalesInvoice and Part
        modelBuilder.Entity<SalesInvoiceItem>(e => {
            e.ToTable("sales_invoice_items");
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
            e.ToTable("notifications");
            e.HasKey(n => n.NotificationId);
            e.Property(n => n.CreatedAt)
             .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'")
             .ValueGeneratedOnAdd();
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

        // Vendor
        modelBuilder.Entity<Vendor>(e => {
            e.HasKey(v => v.Id);
            e.ToTable("vendors");
        });

        // PurchaseInvoice → Vendor
        modelBuilder.Entity<PurchaseInvoice>(e => {
            e.HasKey(p => p.Id);
            e.ToTable("purchase_invoices");
            e.HasOne<Vendor>()
             .WithMany()
             .HasForeignKey(p => p.VendorId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // PurchaseInvoiceItem → PurchaseInvoice and Part
        modelBuilder.Entity<PurchaseInvoiceItem>(e => {
            e.HasKey(p => p.Id);
            e.ToTable("purchase_invoice_items");
            e.HasOne(p => p.PurchaseInvoice)
             .WithMany(pi => pi.Items)
             .HasForeignKey(p => p.PurchaseInvoiceId)
             .OnDelete(DeleteBehavior.Cascade);
            e.HasOne<Part>()
             .WithMany()
             .HasForeignKey(p => p.PartId)
             .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static string ToSnakeCase(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return input;
        }

        var builder = new System.Text.StringBuilder();
        for (var i = 0; i < input.Length; i++)
        {
            var c = input[i];
            if (char.IsUpper(c))
            {
                if (i > 0)
                {
                    builder.Append('_');
                }

                builder.Append(char.ToLowerInvariant(c));
            }
            else
            {
                builder.Append(c);
            }
        }

        return builder.ToString();
    }
}
