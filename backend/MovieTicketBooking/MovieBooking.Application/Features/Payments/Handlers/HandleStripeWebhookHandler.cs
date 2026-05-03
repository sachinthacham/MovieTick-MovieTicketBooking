using MediatR;
using MovieBooking.Application.Features.Bookings.Commands;
using MovieBooking.Application.Features.Payments.Commands;
using MovieBooking.Application.Interfaces;
using MovieBooking.Domain.Enums;

namespace MovieBooking.Application.Features.Payments.Handlers;

public class HandleStripeWebhookHandler : IRequestHandler<HandleStripeWebhookCommand, bool>
{
    private readonly IPaymentService _paymentService;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IMediator _mediator;

    public HandleStripeWebhookHandler(
        IPaymentService paymentService,
        IPaymentRepository paymentRepository,
        IBookingRepository bookingRepository,
        IMediator mediator)
    {
        _paymentService = paymentService;
        _paymentRepository = paymentRepository;
        _bookingRepository = bookingRepository;
        _mediator = mediator;
    }

    public async Task<bool> Handle(HandleStripeWebhookCommand request, CancellationToken cancellationToken)
    {
        var (eventType, paymentIntentId, chargeId) = _paymentService.ConstructWebhookEvent(request.Payload, request.StripeSignature);

        switch (eventType)
        {
            case "payment_intent.succeeded":
                await _mediator.Send(new ConfirmBookingCommand(paymentIntentId, chargeId), cancellationToken);
                break;

            case "payment_intent.payment_failed":
                var payment = await _paymentRepository.GetByPaymentIntentAsync(paymentIntentId);
                if (payment != null)
                {
                    payment.Status = PaymentStatus.Failed;
                    await _paymentRepository.UpdateAsync(payment);

                    var booking = await _bookingRepository.GetByIdAsync(payment.BookingId);
                    if (booking != null)
                    {
                        booking.Status = BookingStatus.Expired;
                        await _bookingRepository.UpdateAsync(booking);
                    }
                }
                break;
        }

        return true;
    }
}
