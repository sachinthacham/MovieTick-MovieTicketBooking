namespace MovieBooking.Application.DTOs.Auth;

public class UserProfileDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string? PhoneNumber { get; set; }
    public string Role { get; set; } = default!;
    public string? ProfileImageUrl { get; set; }
    public string? Bio { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? SocialProvider { get; set; }
    public DateTime CreatedAt { get; set; }
}
