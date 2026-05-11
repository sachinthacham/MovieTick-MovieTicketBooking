using MovieBooking.Domain.Entities;

namespace MovieBooking.Application.Interfaces;

public interface IShowFormatRepository
{
    Task AddAsync(ShowFormat format);
    Task<List<ShowFormat>> GetAllAsync();
    Task<ShowFormat?> GetByIdAsync(Guid id);
    Task UpdateAsync(ShowFormat format);
    Task DeleteAsync(ShowFormat format);
}
