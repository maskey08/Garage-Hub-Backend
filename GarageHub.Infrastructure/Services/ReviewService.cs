using System;
using System.Collections.Generic;
using System.Text;

using GarageHub.Application.DTOs.Review;
using GarageHub.Application.Interfaces;
using GarageHub.Domain.Entities;
using GarageHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GarageHub.Infrastructure.Services;

public class ReviewService : IReviewService
{
    private readonly AppDbContext _db;
    public ReviewService(AppDbContext db) => _db = db;

    public async Task<Review> SubmitAsync(int customerId, ReviewCreateDto dto)
    {
        var appointment = await _db.Appointments
            .FirstOrDefaultAsync(a => a.AppointmentId == dto.AppointmentId && a.CustomerId == customerId)
            ?? throw new Exception("Appointment not found or does not belong to you.");

        if (appointment.Status != "completed")
            throw new Exception("You can only review completed appointments.");

        var existing = await _db.Reviews.AnyAsync(r => r.AppointmentId == dto.AppointmentId);
        if (existing) throw new Exception("Review already submitted for this appointment.");

        var review = new Review
        {
            CustomerId = customerId,
            AppointmentId = dto.AppointmentId,
            Rating = dto.Rating,
            Comment = dto.Comment
        };
        _db.Reviews.Add(review);
        await _db.SaveChangesAsync();
        return review;
    }

    public async Task<IEnumerable<Review>> GetByCustomerAsync(int customerId)
        => await _db.Reviews
            .Include(r => r.Appointment)
            .Where(r => r.CustomerId == customerId)
            .OrderByDescending(r => r.ReviewedAt)
            .ToListAsync();
}
