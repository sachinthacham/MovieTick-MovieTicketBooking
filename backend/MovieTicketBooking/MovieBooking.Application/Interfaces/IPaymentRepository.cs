using MovieBooking.Domain.Entities;

namespace MovieBooking.Application.Interfaces;

public interface IPaymentRepository
{
    Task AddAsync(Payment payment);
    Task<Payment?> GetByBookingAsync(Guid bookingId);
    Task<Payment?> GetByPaymentIntentAsync(string paymentIntentId);
    Task UpdateAsync(Payment payment);
}
