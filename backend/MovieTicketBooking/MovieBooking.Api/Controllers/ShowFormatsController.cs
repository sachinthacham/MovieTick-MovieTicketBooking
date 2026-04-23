using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieBooking.Application.Common.Responses;
using MovieBooking.Application.DTOs.Showtimes;
using MovieBooking.Application.Features.ShowFormats.Commands;
using MovieBooking.Application.Features.ShowFormats.Queries;

namespace MovieBooking.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/show-formats")]
public class ShowFormatsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ShowFormatsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllShowFormatsQuery());
        return Ok(ApiResponse<List<ShowFormatDto>>.SuccessResponse(result));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateShowFormatDto dto)
    {
        var result = await _mediator.Send(new CreateShowFormatCommand(dto.Name, dto.Description));
        return Ok(ApiResponse<ShowFormatDto>.SuccessResponse(result, "Show format created."));
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] CreateShowFormatDto dto)
    {
        var result = await _mediator.Send(new UpdateShowFormatCommand(id, dto.Name, dto.Description));
        return Ok(ApiResponse<ShowFormatDto>.SuccessResponse(result, "Show format updated."));
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteShowFormatCommand(id));
        return Ok(ApiResponse<bool>.SuccessResponse(true, "Show format deleted."));
    }
}
