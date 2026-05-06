using MediatR;
using MovieBooking.Application.Features.Bookings.Commands;
using MovieBooking.Application.Interfaces;
using MovieBooking.Domain.Entities;
using MovieBooking.Domain.Enums;

namespace MovieBooking.Application.Features.Bookings.Handlers;

public class CancelBookingHandler : IRequestHandler<CancelBookingCommand, bool>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IBookingItemRepository _bookingItemRepository;
    private readonly IShowtimeSeatRepository _showtimeSeatRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly IPaymentService _paymentService;
    private readonly INotificationService _notificationService;
    private readonly ISeatAvailabilityNotifier _notifier;

    public CancelBookingHandler(
        IBookingRepository bookingRepository,
        IPaymentRepository paymentRepository,
        IBookingItemRepository bookingItemRepository,
        IShowtimeSeatRepository showtimeSeatRepository,
        INotificationRepository notificationRepository,
        IPaymentService paymentService,
        INotificationService notificationService,
        ISeatAvailabilityNotifier notifier)
    {
        _bookingRepository = bookingRepository;
        _paymentRepository = paymentRepository;
        _bookingItemRepository = bookingItemRepository;
        _showtimeSeatRepository = showtimeSeatRepository;
        _notificationRepository = notificationRepository;
        _paymentService = paymentService;
        _notificationService = notificationService;
        _notifier = notifier;
    }

    public async Task<bool> Handle(CancelBookingCommand request, CancellationToken cancellationToken)
    {
        var booking = await _bookingRepository.GetByIdAsync(request.BookingId)
            ?? throw new KeyNotFoundException("Booking not found.");

        if (booking.Status == BookingStatus.Cancelled)
            throw new InvalidOperationException("Booking is already cancelled.");

        if (request.RequestingUserRole != "Admin" && booking.UserId != request.RequestingUserId)
            throw new UnauthorizedAccessException("You cannot cancel this booking.");

        booking.Status = BookingStatus.Cancelled;
        booking.CancelledAt = DateTime.UtcNow;
        booking.CancellationReason = request.Reason;
        await _bookingRepository.UpdateAsync(booking);

        var items = await _bookingItemRepository.GetByBookingAsync(booking.Id);
        var seatIds = items.Select(i => i.ShowtimeSeatId).ToList();
        var seats = await _showtimeSeatRepository.GetByIdsAsync(seatIds);

        foreach (var seat in seats)
        {
            seat.Status = SeatStatus.Available;
            seat.BlockedUntil = null;
            seat.LockedBySession = null;
        }
        await _showtimeSeatRepository.UpdateRangeAsync(seats);

        foreach (var seat in seats)
            await _notifier.NotifySeatStatusChangedAsync(booking.ShowtimeId, seat.SeatId, "Available");

        var payment = await _paymentRepository.GetByBookingAsync(booking.Id);
        if (payment != null && payment.Status == PaymentStatus.Succeeded && payment.StripeChargeId != null)
        {
            await _paymentService.RefundPaymentAsync(payment.StripeChargeId);
            payment.Status = PaymentStatus.Refunded;
            await _paymentRepository.UpdateAsync(payment);

            booking.Status = BookingStatus.Refunded;
            await _bookingRepository.UpdateAsync(booking);
        }

        var notification = new Notification
        {
            UserId = booking.UserId,
            Title = "Booking Cancelled",
            Message = $"Your booking {booking.BookingReference} has been cancelled.",
            Type = "BookingCancellation",
            IsRead = false,
            MetaData = System.Text.Json.JsonSerializer.Serialize(new { bookingId = booking.Id, reference = booking.BookingReference })
        };
        await _notificationRepository.AddAsync(notification);

        await _notificationService.SendCancellationAsync(booking);

        return true;
    }
}
