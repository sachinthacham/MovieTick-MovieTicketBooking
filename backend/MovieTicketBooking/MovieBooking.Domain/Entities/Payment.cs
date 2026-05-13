using MovieBooking.Domain.Common;
using MovieBooking.Domain.Enums;

namespace MovieBooking.Domain.Entities;

public class Payment : BaseEntity
{
    public Guid BookingId { get; set; }
    public Booking Booking { get; set; } = default!;

    public string StripePaymentIntentId { get; set; } = default!;
    public string? StripeChargeId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "usd";
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    public string? FailureReason { get; set; }
    public DateTime? PaidAt { get; set; }
}
