using MediatR;
using MovieBooking.Application.DTOs.Tickets;

namespace MovieBooking.Application.Features.Tickets.Commands;

public record ValidateTicketCommand(string TicketNumber) : IRequest<TicketDto>;
