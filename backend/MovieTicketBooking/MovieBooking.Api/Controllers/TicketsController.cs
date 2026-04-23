using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieBooking.Application.Common.Responses;
using MovieBooking.Application.DTOs.Tickets;
using MovieBooking.Application.Features.Tickets.Commands;
using MovieBooking.Application.Features.Tickets.Queries;
using System.Security.Claims;

namespace MovieBooking.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/tickets")]
[ApiVersion("1.0")]
public class TicketsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TicketsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize]
    [HttpGet("booking/{bookingId:guid}")]
    public async Task<IActionResult> GetByBooking(Guid bookingId)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var role = User.FindFirstValue(ClaimTypes.Role) ?? "User";
        var result = await _mediator.Send(new GetTicketsByBookingQuery(bookingId, userId, role));
        return Ok(ApiResponse<List<TicketDto>>.SuccessResponse(result));
    }

    [Authorize(Roles = "Admin")]
    [HttpPatch("{ticketNumber}/validate")]
    public async Task<IActionResult> Validate(string ticketNumber)
    {
        var result = await _mediator.Send(new ValidateTicketCommand(ticketNumber));
        return Ok(ApiResponse<TicketDto>.SuccessResponse(result, "Ticket validated."));
    }
}
