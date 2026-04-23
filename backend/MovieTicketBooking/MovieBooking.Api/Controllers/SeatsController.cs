using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieBooking.Application.Common.Responses;
using MovieBooking.Application.DTOs.Seats;
using MovieBooking.Application.Features.Seats.Commands;
using MovieBooking.Application.Features.Seats.Queries;

namespace MovieBooking.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/seats")]
public class SeatsController : ControllerBase
{
    private readonly IMediator _mediator;

    public SeatsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("screen/{screenId:guid}")]
    public async Task<IActionResult> GetByScreen(Guid screenId)
    {
        var result = await _mediator.Send(new GetSeatsByScreenQuery(screenId));
        return Ok(ApiResponse<List<SeatDto>>.SuccessResponse(result));
    }

    [HttpGet("availability/{showtimeId:guid}")]
    public async Task<IActionResult> GetAvailability(Guid showtimeId)
    {
        var result = await _mediator.Send(new GetSeatAvailabilityQuery(showtimeId));
        return Ok(ApiResponse<List<SeatAvailabilityDto>>.SuccessResponse(result));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("bulk")]
    public async Task<IActionResult> BulkCreate([FromBody] BulkCreateSeatsDto dto)
    {
        var result = await _mediator.Send(new BulkCreateSeatsCommand(dto.ScreenId, dto.Rows));
        return Ok(ApiResponse<List<SeatDto>>.SuccessResponse(result, $"{result.Count} seats created."));
    }

    [Authorize(Roles = "Admin")]
    [HttpPatch("{id:guid}/block")]
    public async Task<IActionResult> Block(Guid id)
    {
        await _mediator.Send(new BlockSeatCommand(id));
        return Ok(ApiResponse<bool>.SuccessResponse(true, "Seat blocked."));
    }

    [Authorize(Roles = "Admin")]
    [HttpPatch("{id:guid}/unblock")]
    public async Task<IActionResult> Unblock(Guid id)
    {
        await _mediator.Send(new UnblockSeatCommand(id));
        return Ok(ApiResponse<bool>.SuccessResponse(true, "Seat unblocked."));
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSeatDto dto)
    {
        var result = await _mediator.Send(new UpdateSeatCommand(id, dto.SeatCategoryId, dto.IsActive));
        return Ok(ApiResponse<SeatDto>.SuccessResponse(result, "Seat updated."));
    }
}
