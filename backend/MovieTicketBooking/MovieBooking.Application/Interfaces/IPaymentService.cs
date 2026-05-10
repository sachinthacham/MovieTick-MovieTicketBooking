namespace MovieBooking.Application.Interfaces;

public interface IPaymentService
{
    Task<(string PaymentIntentId, string ClientSecret)> CreatePaymentIntentAsync(decimal amount, string currency, Dictionary<string, string> metadata);
    Task<string?> RefundPaymentAsync(string chargeId, decimal? amount = null);
    (string EventType, string PaymentIntentId, string? ChargeId) ConstructWebhookEvent(string payload, string signature);
}
