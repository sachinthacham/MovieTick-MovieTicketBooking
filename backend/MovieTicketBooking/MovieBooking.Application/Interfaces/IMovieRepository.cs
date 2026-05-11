using MovieBooking.Domain.Entities;

namespace MovieBooking.Application.Interfaces;

public interface IMovieRepository
{
    Task AddAsync(Movie movie);
    Task<(List<Movie> Items, int TotalCount)> GetAllAsync(int page, int pageSize, string? search, Guid? genreId, Guid? languageId, bool? isFeatured, bool? isComingSoon, bool? isActive, string? sortBy = null, string? cityFilter = null, Guid? formatId = null);
    Task<Movie?> GetByIdAsync(Guid id);
    Task UpdateAsync(Movie movie);
    Task DeleteAsync(Movie movie);
}
