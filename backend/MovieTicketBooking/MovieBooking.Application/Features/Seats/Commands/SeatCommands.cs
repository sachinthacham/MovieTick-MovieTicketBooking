using MediatR;
using MovieBooking.Application.DTOs.Seats;

namespace MovieBooking.Application.Features.Seats.Commands;

public record BulkCreateSeatsCommand(Guid ScreenId, List<SeatRowConfig> Rows) : IRequest<List<SeatDto>>;
public record UpdateSeatCommand(Guid SeatId, Guid? SeatCategoryId, bool? IsActive) : IRequest<SeatDto>;
public record BlockSeatCommand(Guid SeatId) : IRequest<bool>;
public record UnblockSeatCommand(Guid SeatId) : IRequest<bool>;
