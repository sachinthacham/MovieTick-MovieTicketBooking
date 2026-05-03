using MediatR;
using MovieBooking.Application.Features.Bookings.Commands;
using MovieBooking.Application.Interfaces;
using MovieBooking.Domain.Entities;
using MovieBooking.Domain.Enums;

namespace MovieBooking.Application.Features.Bookings.Handlers;

public class ConfirmBookingHandler : IRequestHandler<ConfirmBookingCommand, bool>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IBookingItemRepository _bookingItemRepository;
    private readonly IShowtimeSeatRepository _showtimeSeatRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly IQrCodeService _qrCodeService;
    private readonly INotificationService _notificationService;
    private readonly ISeatAvailabilityNotifier _notifier;

    public ConfirmBookingHandler(
        IBookingRepository bookingRepository,
        IPaymentRepository paymentRepository,
        IBookingItemRepository bookingItemRepository,
        IShowtimeSeatRepository showtimeSeatRepository,
        ITicketRepository ticketRepository,
        INotificationRepository notificationRepository,
        IQrCodeService qrCodeService,
        INotificationService notificationService,
        ISeatAvailabilityNotifier notifier)
    {
        _bookingRepository = bookingRepository;
        _paymentRepository = paymentRepository;
        _bookingItemRepository = bookingItemRepository;
        _showtimeSeatRepository = showtimeSeatRepository;
        _ticketRepository = ticketRepository;
        _notificationRepository = notificationRepository;
        _qrCodeService = qrCodeService;
        _notificationService = notificationService;
        _notifier = notifier;
    }

    public async Task<bool> Handle(ConfirmBookingCommand request, CancellationToken cancellationToken)
    {
        var payment = await _paymentRepository.GetByPaymentIntentAsync(request.PaymentIntentId);
        if (payment == null) return false;

        var booking = await _bookingRepository.GetByIdAsync(payment.BookingId);
        if (booking == null) return false;

        payment.Status = PaymentStatus.Succeeded;
        payment.StripeChargeId = request.ChargeId;
        payment.PaidAt = DateTime.UtcNow;
        await _paymentRepository.UpdateAsync(payment);

        booking.Status = BookingStatus.Confirmed;
        booking.ConfirmedAt = DateTime.UtcNow;
        await _bookingRepository.UpdateAsync(booking);

        var items = await _bookingItemRepository.GetByBookingAsync(booking.Id);

        var seatIds = items.Select(i => i.ShowtimeSeatId).ToList();
        var showtimeSeats = await _showtimeSeatRepository.GetByIdsAsync(seatIds);

        foreach (var seat in showtimeSeats)
        {
            seat.Status = SeatStatus.Booked;
            seat.BlockedUntil = null;
            seat.LockedBySession = null;
        }
        await _showtimeSeatRepository.UpdateRangeAsync(showtimeSeats);

        foreach (var seat in showtimeSeats)
            await _notifier.NotifySeatStatusChangedAsync(booking.ShowtimeId, seat.SeatId, "Booked");

        var tickets = items.Select(item => new Ticket
        {
            BookingId = booking.Id,
            BookingItemId = item.Id,
            TicketNumber = GenerateTicketNumber(),
            QrCodeData = _qrCodeService.GenerateQrCodeBase64($"{booking.BookingReference}:{item.SeatNumber}"),
            IsUsed = false
        }).ToList();

        await _ticketRepository.AddRangeAsync(tickets);

        var notification = new Notification
        {
            UserId = booking.UserId,
            Title = "Booking Confirmed",
            Message = $"Your booking {booking.BookingReference} has been confirmed. Enjoy the show!",
            Type = "BookingConfirmation",
            IsRead = false,
            MetaData = System.Text.Json.JsonSerializer.Serialize(new { bookingId = booking.Id, reference = booking.BookingReference })
        };
        await _notificationRepository.AddAsync(notification);

        await _notificationService.SendBookingConfirmationAsync(booking);

        return true;
    }

    private static string GenerateTicketNumber()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        var part1 = new string(Enumerable.Repeat(chars, 4).Select(s => s[random.Next(s.Length)]).ToArray());
        var part2 = new string(Enumerable.Repeat(chars, 4).Select(s => s[random.Next(s.Length)]).ToArray());
        return $"TKT-{part1}-{part2}";
    }
}
