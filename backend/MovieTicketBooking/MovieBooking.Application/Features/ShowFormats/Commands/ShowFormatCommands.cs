using MediatR;
using MovieBooking.Application.DTOs.Showtimes;

namespace MovieBooking.Application.Features.ShowFormats.Commands;

public record CreateShowFormatCommand(string Name, string? Description) : IRequest<ShowFormatDto>;
public record UpdateShowFormatCommand(Guid Id, string? Name, string? Description) : IRequest<ShowFormatDto>;
public record DeleteShowFormatCommand(Guid Id) : IRequest<bool>;
