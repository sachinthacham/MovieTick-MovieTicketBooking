using MovieBooking.Domain.Entities;

namespace MovieBooking.Application.Interfaces;

public interface ITheaterFacilityRepository
{
    Task AddAsync(TheaterFacility facility);
    Task<List<TheaterFacility>> GetByTheaterAsync(Guid theaterId);
    Task<TheaterFacility?> GetByIdAsync(Guid id);
    Task DeleteAsync(TheaterFacility facility);
}
