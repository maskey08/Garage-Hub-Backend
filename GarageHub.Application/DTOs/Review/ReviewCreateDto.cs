namespace GarageHub.Application.DTOs.Review;

public class ReviewCreateDto
{
    public int AppointmentId { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
}
