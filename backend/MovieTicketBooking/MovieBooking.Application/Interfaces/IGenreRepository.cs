using MovieBooking.Domain.Entities;

namespace MovieBooking.Application.Interfaces;

public interface IGenreRepository
{
    Task AddAsync(Genre genre);
    Task<List<Genre>> GetAllAsync();
    Task<Genre?> GetByIdAsync(Guid id);
    Task<Genre?> GetByNameAsync(string name);
    Task UpdateAsync(Genre genre);
    Task DeleteAsync(Genre genre);
}
