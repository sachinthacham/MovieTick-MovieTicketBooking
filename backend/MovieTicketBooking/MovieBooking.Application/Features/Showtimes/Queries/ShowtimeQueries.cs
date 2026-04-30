using MediatR;
using MovieBooking.Application.DTOs.Showtimes;

namespace MovieBooking.Application.Features.Showtimes.Queries;

public record GetShowtimesByMovieQuery(Guid MovieId, DateTime? Date = null) : IRequest<List<ShowtimeDto>>;
public record GetShowtimesByTheaterQuery(Guid TheaterId, DateTime? Date = null) : IRequest<List<ShowtimeDto>>;
public record GetShowtimesByScreenQuery(Guid ScreenId, DateTime? Date = null) : IRequest<List<ShowtimeDto>>;
public record GetShowtimeByIdQuery(Guid Id) : IRequest<ShowtimeDto>;
public record GetRecentShowtimesQuery(int Take = 20) : IRequest<List<ShowtimeDto>>;
public record GetShowtimeCountQuery : IRequest<int>;
