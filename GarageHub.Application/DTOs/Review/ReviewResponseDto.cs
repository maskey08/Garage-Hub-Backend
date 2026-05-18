namespace GarageHub.Application.DTOs.Review;

public class ReviewResponseDto
{
    public int ReviewId { get; set; }
    public int CustomerId { get; set; }
    public int AppointmentId { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public DateTime ReviewedAt { get; set; }
}
