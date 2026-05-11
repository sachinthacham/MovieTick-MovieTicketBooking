using MovieBooking.Domain.Entities;

namespace MovieBooking.Application.Interfaces;

public interface ILanguageRepository
{
    Task AddAsync(Language language);
    Task<List<Language>> GetAllAsync();
    Task<Language?> GetByIdAsync(Guid id);
    Task UpdateAsync(Language language);
    Task DeleteAsync(Language language);
}
