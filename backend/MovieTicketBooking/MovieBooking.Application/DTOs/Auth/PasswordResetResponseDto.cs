namespace MovieBooking.Application.DTOs.Auth;

public class PasswordResetResponseDto
{
    public string ResetToken { get; set; } = default!;
    public DateTime ExpiresAt { get; set; }
}
