using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieBooking.Application.Common.Responses;
using MovieBooking.Application.DTOs.Notifications;
using MovieBooking.Application.Features.Notifications.Commands;
using MovieBooking.Application.Features.Notifications.Queries;
using System.Security.Claims;

namespace MovieBooking.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/notifications")]
[ApiVersion("1.0")]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public NotificationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetMyNotifications(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _mediator.Send(new GetUserNotificationsQuery(userId, page, pageSize));
        return Ok(ApiResponse<PagedNotificationsDto>.SuccessResponse(result));
    }

    [HttpPatch("read")]
    public async Task<IActionResult> MarkRead([FromBody] MarkReadRequestDto dto)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _mediator.Send(new MarkNotificationsReadCommand(userId, dto.NotificationIds));
        return Ok(ApiResponse<bool>.SuccessResponse(result));
    }
}

public class MarkReadRequestDto
{
    public List<Guid> NotificationIds { get; set; } = [];
}
