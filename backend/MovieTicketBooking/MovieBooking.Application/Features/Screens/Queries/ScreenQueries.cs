using MediatR;
using MovieBooking.Application.DTOs.Screens;

namespace MovieBooking.Application.Features.Screens.Queries;

public record GetScreensByTheaterQuery(Guid TheaterId) : IRequest<List<ScreenDto>>;
public record GetScreenByIdQuery(Guid Id) : IRequest<ScreenDto>;
