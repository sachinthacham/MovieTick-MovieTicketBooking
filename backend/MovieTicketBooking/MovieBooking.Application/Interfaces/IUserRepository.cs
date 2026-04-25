using MovieBooking.Domain.Entities;

namespace MovieBooking.Application.Interfaces;

public interface IUserRepository
{
    Task AddAsync(User user);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetBySocialIdAsync(string provider, string socialId);
    Task UpdateAsync(User user);
    Task<(List<User> Items, int TotalCount)> GetAllAsync(int page, int pageSize, string? search, string? role);
}