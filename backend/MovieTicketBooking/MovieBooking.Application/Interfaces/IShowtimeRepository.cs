using MovieBooking.Domain.Entities;

namespace MovieBooking.Application.Interfaces;

public interface IShowtimeRepository
{
    Task AddAsync(Showtime showtime);
    Task<Showtime?> GetByIdAsync(Guid id);
    Task<List<Showtime>> GetByMovieAsync(Guid movieId, DateTime? date);
    Task<List<Showtime>> GetByTheaterAsync(Guid theaterId, DateTime? date);
    Task<List<Showtime>> GetByScreenAsync(Guid screenId, DateTime? date);
    Task<List<Showtime>> GetRecentAsync(int take);
    Task<int> CountAsync(CancellationToken cancellationToken = default);
    Task UpdateAsync(Showtime showtime);
    Task DeleteAsync(Showtime showtime);
}
