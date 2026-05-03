using MediatR;
using MovieBooking.Application.DTOs.Bookings;
using MovieBooking.Application.Features.Bookings.Commands;
using MovieBooking.Application.Interfaces;
using MovieBooking.Domain.Entities;
using MovieBooking.Domain.Enums;

namespace MovieBooking.Application.Features.Bookings.Handlers;

public class CreateBookingHandler : IRequestHandler<CreateBookingCommand, CreateBookingResultDto>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IBookingItemRepository _bookingItemRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IShowtimeSeatRepository _showtimeSeatRepository;
    private readonly IShowtimeRepository _showtimeRepository;
    private readonly IPaymentService _paymentService;
    private readonly IBookingSettings _settings;

    public CreateBookingHandler(
        IBookingRepository bookingRepository,
        IBookingItemRepository bookingItemRepository,
        IPaymentRepository paymentRepository,
        IShowtimeSeatRepository showtimeSeatRepository,
        IShowtimeRepository showtimeRepository,
        IPaymentService paymentService,
        IBookingSettings settings)
    {
        _bookingRepository = bookingRepository;
        _bookingItemRepository = bookingItemRepository;
        _paymentRepository = paymentRepository;
        _showtimeSeatRepository = showtimeSeatRepository;
        _showtimeRepository = showtimeRepository;
        _paymentService = paymentService;
        _settings = settings;
    }

    public async Task<CreateBookingResultDto> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
    {
        var showtime = await _showtimeRepository.GetByIdAsync(request.ShowtimeId)
            ?? throw new KeyNotFoundException("Showtime not found.");

        var seats = await _showtimeSeatRepository.GetByIdsAsync(request.SeatIds);

        foreach (var seat in seats)
        {
            if (seat.Status != SeatStatus.Reserved || seat.LockedBySession != request.SessionId)
                throw new InvalidOperationException($"Seat {seat.SeatId} is not reserved for this session.");
        }

        var expiryMinutes = _settings.ExpiryMinutes;
        var bookingRef = GenerateReference();

        var booking = new Booking
        {
            UserId = request.UserId,
            ShowtimeId = request.ShowtimeId,
            BookingReference = bookingRef,
            Status = BookingStatus.Pending,
            ExpiresAt = DateTime.UtcNow.AddMinutes(expiryMinutes)
        };

        var items = new List<BookingItem>();
        decimal total = 0m;

        foreach (var seat in seats)
        {
            var price = showtime.Pricings.FirstOrDefault(p => p.SeatCategoryId == seat.Seat.SeatCategory.Id)?.Price
                        ?? seat.Seat.SeatCategory.DefaultPrice;

            var item = new BookingItem
            {
                BookingId = booking.Id,
                ShowtimeSeatId = seat.Id,
                SeatNumber = seat.Seat.SeatNumber,
                RowLabel = seat.Seat.Row,
                SeatCategoryName = seat.Seat.SeatCategory.Name,
                Price = price
            };
            items.Add(item);
            total += price;
        }

        booking.TotalAmount = total;

        await _bookingRepository.AddAsync(booking);
        await _bookingItemRepository.AddRangeAsync(items);

        var metadata = new Dictionary<string, string>
        {
            ["bookingId"] = booking.Id.ToString(),
            ["bookingReference"] = bookingRef,
            ["userId"] = request.UserId.ToString()
        };

        var (paymentIntentId, clientSecret) = await _paymentService.CreatePaymentIntentAsync(total, "usd", metadata);

        var payment = new Payment
        {
            BookingId = booking.Id,
            StripePaymentIntentId = paymentIntentId,
            Amount = total,
            Currency = "usd",
            Status = PaymentStatus.Pending
        };

        await _paymentRepository.AddAsync(payment);

        return new CreateBookingResultDto
        {
            BookingId = booking.Id,
            BookingReference = bookingRef,
            TotalAmount = total,
            StripeClientSecret = clientSecret,
            StripePaymentIntentId = paymentIntentId
        };
    }

    private static string GenerateReference()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, 8).Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
