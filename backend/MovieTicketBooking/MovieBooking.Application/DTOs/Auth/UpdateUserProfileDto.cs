namespace MovieBooking.Application.DTOs.Auth;

public class UpdateUserProfileDto
{
    public string? FullName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Bio { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
}
