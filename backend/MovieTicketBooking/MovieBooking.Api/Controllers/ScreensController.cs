using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieBooking.Application.Common.Responses;
using MovieBooking.Application.DTOs.Screens;
using MovieBooking.Application.Features.Screens.Commands;
using MovieBooking.Application.Features.Screens.Queries;

namespace MovieBooking.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/screens")]
public class ScreensController : ControllerBase
{
    private readonly IMediator _mediator;

    public ScreensController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("theater/{theaterId:guid}")]
    public async Task<IActionResult> GetByTheater(Guid theaterId)
    {
        var result = await _mediator.Send(new GetScreensByTheaterQuery(theaterId));
        return Ok(ApiResponse<List<ScreenDto>>.SuccessResponse(result));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetScreenByIdQuery(id));
        return Ok(ApiResponse<ScreenDto>.SuccessResponse(result));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateScreenDto dto)
    {
        var result = await _mediator.Send(new CreateScreenCommand(dto.TheaterId, dto.Name, dto.ScreenType));
        return Ok(ApiResponse<ScreenDto>.SuccessResponse(result, "Screen created."));
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateScreenDto dto)
    {
        var result = await _mediator.Send(new UpdateScreenCommand(id, dto.Name, dto.ScreenType, dto.IsActive));
        return Ok(ApiResponse<ScreenDto>.SuccessResponse(result, "Screen updated."));
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteScreenCommand(id));
        return Ok(ApiResponse<bool>.SuccessResponse(true, "Screen deleted."));
    }
}
