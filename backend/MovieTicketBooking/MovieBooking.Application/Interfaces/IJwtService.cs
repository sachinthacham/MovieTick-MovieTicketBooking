namespace MovieBooking.Application.Interfaces;

public interface IJwtService
{
    string GenerateToken(Guid userId, string email, string role);
    string GenerateRefreshToken();
}