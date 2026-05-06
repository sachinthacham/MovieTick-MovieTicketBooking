using MediatR;
using MovieBooking.Application.DTOs.Bookings;
using MovieBooking.Application.Features.Bookings.Queries;
using MovieBooking.Application.Interfaces;

namespace MovieBooking.Application.Features.Bookings.Handlers;

public class GetUserBookingsHandler : IRequestHandler<GetUserBookingsQuery, PagedBookingsDto>
{
    private readonly IBookingRepository _bookingRepository;

    public GetUserBookingsHandler(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task<PagedBookingsDto> Handle(GetUserBookingsQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _bookingRepository.GetByUserAsync(
            request.UserId, request.Page, request.PageSize, request.Status);

        return new PagedBookingsDto
        {
            Items = items.Select(b => BookingMappingHelper.ToDto(b, [])).ToList(),
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}
