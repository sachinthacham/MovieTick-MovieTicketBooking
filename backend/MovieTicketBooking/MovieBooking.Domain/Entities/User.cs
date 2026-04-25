using MovieBooking.Domain.Common;

namespace MovieBooking.Domain.Entities;

public class User : BaseEntity
{
    public string FullName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string? PasswordHash { get; set; }
    public string? PhoneNumber { get; set; }
    public string Role { get; set; } = "User";
    public string? ProfileImageUrl { get; set; }
    public string? Bio { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? SocialProvider { get; set; }
    public string? SocialId { get; set; }
    public string? PasswordResetToken { get; set; }
    public DateTime? PasswordResetTokenExpiry { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
    public ICollection<MovieRating> MovieRatings { get; set; } = [];
    public ICollection<MovieReview> MovieReviews { get; set; } = [];
    public ICollection<TheaterRating> TheaterRatings { get; set; } = [];
    public ICollection<Booking> Bookings { get; set; } = [];
    public ICollection<Notification> Notifications { get; set; } = [];
}