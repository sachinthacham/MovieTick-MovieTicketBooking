using MediatR;
using MovieBooking.Application.DTOs.Showtimes;

namespace MovieBooking.Application.Features.Showtimes.Commands;

public record CreateShowtimeCommand(
    Guid MovieId,
    Guid ScreenId,
    Guid ShowFormatId,
    Guid LanguageId,
    DateTime StartTime) : IRequest<ShowtimeDto>;

public record UpdateShowtimeCommand(
    Guid Id,
    Guid? ShowFormatId,
    Guid? LanguageId,
    DateTime? StartTime,
    string? Status) : IRequest<ShowtimeDto>;

public record DeleteShowtimeCommand(Guid Id) : IRequest<bool>;

public record SetShowtimePricingCommand(Guid ShowtimeId, Guid SeatCategoryId, decimal Price) : IRequest<ShowtimePricingDto>;
