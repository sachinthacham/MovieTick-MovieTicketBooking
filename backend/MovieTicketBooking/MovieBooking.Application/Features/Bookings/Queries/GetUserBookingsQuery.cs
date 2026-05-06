using MediatR;
using MovieBooking.Application.DTOs.Bookings;
using MovieBooking.Domain.Enums;

namespace MovieBooking.Application.Features.Bookings.Queries;

public record GetUserBookingsQuery(
    Guid UserId,
    int Page = 1,
    int PageSize = 10,
    BookingStatus? Status = null
) : IRequest<PagedBookingsDto>;
