using MovieBooking.Domain.Entities;


namespace MovieBooking.Application.Interfaces;

public interface IUserRepository
{
    Task AddAsync(User user);
    Task<User?> GetByEmailAsync(string email);
}