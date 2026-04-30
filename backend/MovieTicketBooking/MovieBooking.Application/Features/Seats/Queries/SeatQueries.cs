using MediatR;
using MovieBooking.Application.DTOs.Seats;

namespace MovieBooking.Application.Features.Seats.Queries;

public record GetSeatsByScreenQuery(Guid ScreenId) : IRequest<List<SeatDto>>;
public record GetSeatAvailabilityQuery(Guid ShowtimeId) : IRequest<List<SeatAvailabilityDto>>;
