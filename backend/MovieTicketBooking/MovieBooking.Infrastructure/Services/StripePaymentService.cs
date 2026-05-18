using Microsoft.Extensions.Configuration;
using MovieBooking.Application.Interfaces;
using Stripe;

namespace MovieBooking.Infrastructure.Services;

public class StripePaymentService : IPaymentService
{
    private readonly string _webhookSecret;

    public StripePaymentService(IConfiguration config)
    {
        var secretKey = config["Stripe:SecretKey"] ?? string.Empty;
        _webhookSecret = config["Stripe:WebhookSecret"] ?? string.Empty;
        StripeConfiguration.ApiKey = secretKey;
    }

    public async Task<(string PaymentIntentId, string ClientSecret)> CreatePaymentIntentAsync(
        decimal amount, string currency, Dictionary<string, string> metadata)
    {
        var options = new PaymentIntentCreateOptions
        {
            Amount = (long)(amount * 100),
            Currency = currency,
            Metadata = metadata,
            AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
            {
                Enabled = true
            }
        };

        var service = new PaymentIntentService();
        var intent = await service.CreateAsync(options);

        return (intent.Id, intent.ClientSecret);
    }

    public async Task<string?> RefundPaymentAsync(string chargeId, decimal? amount = null)
    {
        var options = new RefundCreateOptions
        {
            Charge = chargeId,
            Amount = amount.HasValue ? (long)(amount.Value * 100) : null
        };

        var service = new RefundService();
        var refund = await service.CreateAsync(options);
        return refund.Id;
    }

    public (string EventType, string PaymentIntentId, string? ChargeId) ConstructWebhookEvent(string payload, string signature)
    {
        var stripeEvent = EventUtility.ConstructEvent(payload, signature, _webhookSecret);

        string? chargeId = null;
        string paymentIntentId = string.Empty;

        if (stripeEvent.Data.Object is PaymentIntent intent)
        {
            paymentIntentId = intent.Id;
            chargeId = intent.LatestChargeId;
        }

        return (stripeEvent.Type, paymentIntentId, chargeId);
    }
}
