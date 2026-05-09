using MediatR;
using MovieBooking.Application.DTOs.Theaters;

namespace MovieBooking.Application.Features.Theaters.Queries;

public record GetAllTheatersQuery(string? City = null, bool? IsActive = null) : IRequest<List<TheaterDto>>;
public record GetTheaterByIdQuery(Guid Id) : IRequest<TheaterDto>;
public record GetTheaterRatingsQuery(Guid TheaterId) : IRequest<List<TheaterRatingDto>>;
