using MediatR;
using MovieBooking.Application.DTOs.Screens;

namespace MovieBooking.Application.Features.Screens.Commands;

public record CreateScreenCommand(Guid TheaterId, string Name, string ScreenType) : IRequest<ScreenDto>;
public record UpdateScreenCommand(Guid Id, string? Name, string? ScreenType, bool? IsActive) : IRequest<ScreenDto>;
public record DeleteScreenCommand(Guid Id) : IRequest<bool>;
