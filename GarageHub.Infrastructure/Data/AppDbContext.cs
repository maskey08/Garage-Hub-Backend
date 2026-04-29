using System;
using System.Collections.Generic;
using System.Text;

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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // User
        modelBuilder.Entity<User>(e => {
            e.HasKey(u => u.UserId);
            e.Property(u => u.Role).HasDefaultValue("customer");
        });

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
        });

        // Notification → User
        modelBuilder.Entity<Notification>(e => {
            e.HasKey(n => n.NotificationId);
            e.HasOne(n => n.User)
             .WithMany(u => u.Notifications)
             .HasForeignKey(n => n.UserId)
             .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
