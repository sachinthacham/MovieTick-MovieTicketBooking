using Microsoft.EntityFrameworkCore;
using MovieBooking.Application.Interfaces;
using MovieBooking.Domain.Entities;
using MovieBooking.Infrastructure.Data;

namespace MovieBooking.Infrastructure.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly ApplicationDbContext _context;

    public PaymentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Payment payment)
    {
        await _context.Payments.AddAsync(payment);
        await _context.SaveChangesAsync();
    }

    public async Task<Payment?> GetByBookingAsync(Guid bookingId)
        => await _context.Payments.FirstOrDefaultAsync(p => p.BookingId == bookingId);

    public async Task<Payment?> GetByPaymentIntentAsync(string paymentIntentId)
        => await _context.Payments.FirstOrDefaultAsync(p => p.StripePaymentIntentId == paymentIntentId);

    public async Task UpdateAsync(Payment payment)
    {
        _context.Payments.Update(payment);
        await _context.SaveChangesAsync();
    }
}
