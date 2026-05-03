using MediatR;

namespace MovieBooking.Application.Features.Payments.Commands;

public record HandleStripeWebhookCommand(string Payload, string StripeSignature) : IRequest<bool>;
