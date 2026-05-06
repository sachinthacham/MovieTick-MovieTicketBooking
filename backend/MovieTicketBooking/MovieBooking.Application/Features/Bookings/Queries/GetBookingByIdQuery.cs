using MediatR;
using MovieBooking.Application.DTOs.Bookings;

namespace MovieBooking.Application.Features.Bookings.Queries;

public record GetBookingByIdQuery(Guid BookingId, Guid RequestingUserId, string RequestingUserRole) : IRequest<BookingDto>;
