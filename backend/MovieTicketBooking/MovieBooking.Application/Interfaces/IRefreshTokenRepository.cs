using MovieBooking.Domain.Entities;

namespace MovieBooking.Application.Interfaces;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken token);
    Task<RefreshToken?> GetByTokenAsync(string token);
    Task UpdateAsync(RefreshToken token);
    Task RevokeAllUserTokensAsync(Guid userId);
}