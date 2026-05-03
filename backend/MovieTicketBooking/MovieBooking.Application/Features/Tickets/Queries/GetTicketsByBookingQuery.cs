using MediatR;
using MovieBooking.Application.DTOs.Tickets;

namespace MovieBooking.Application.Features.Tickets.Queries;

public record GetTicketsByBookingQuery(Guid BookingId, Guid RequestingUserId, string RequestingUserRole) : IRequest<List<TicketDto>>;
