using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieBooking.Application.Common.Responses;
using MovieBooking.Application.Features.Payments.Commands;

namespace MovieBooking.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/payments")]
[ApiVersion("1.0")]
public class PaymentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PaymentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [AllowAnonymous]
    [HttpPost("webhook")]
    public async Task<IActionResult> StripeWebhook()
    {
        var payload = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        var signature = Request.Headers["Stripe-Signature"].ToString();

        if (string.IsNullOrWhiteSpace(payload) || string.IsNullOrWhiteSpace(signature))
        {
            return BadRequest(ApiResponse<string>.FailureResponse("Invalid webhook payload or signature."));
        }

        try
        {
            var result = await _mediator.Send(new HandleStripeWebhookCommand(payload, signature));
            return Ok(new { received = result });
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<string>.FailureResponse(ex.Message));
        }
    }
}
