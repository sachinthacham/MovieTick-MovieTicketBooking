using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieBooking.Application.Common.Responses;
using MovieBooking.Application.DTOs.Showtimes;
using MovieBooking.Application.Features.Showtimes.Commands;
using MovieBooking.Application.Features.Showtimes.Queries;

namespace MovieBooking.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/showtimes")]
public class ShowtimesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ShowtimesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("admin/recent")]
    public async Task<IActionResult> GetAdminRecent([FromQuery] int take = 20)
    {
        var result = await _mediator.Send(new GetRecentShowtimesQuery(take));
        return Ok(ApiResponse<List<ShowtimeDto>>.SuccessResponse(result));
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("admin/count")]
    public async Task<IActionResult> GetAdminCount()
    {
        var count = await _mediator.Send(new GetShowtimeCountQuery());
        return Ok(ApiResponse<int>.SuccessResponse(count));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetShowtimeByIdQuery(id));
        return Ok(ApiResponse<ShowtimeDto>.SuccessResponse(result));
    }

    [HttpGet("movie/{movieId:guid}")]
    public async Task<IActionResult> GetByMovie(Guid movieId, [FromQuery] DateTime? date = null)
    {
        var result = await _mediator.Send(new GetShowtimesByMovieQuery(movieId, date));
        return Ok(ApiResponse<List<ShowtimeDto>>.SuccessResponse(result));
    }

    [HttpGet("theater/{theaterId:guid}")]
    public async Task<IActionResult> GetByTheater(Guid theaterId, [FromQuery] DateTime? date = null)
    {
        var result = await _mediator.Send(new GetShowtimesByTheaterQuery(theaterId, date));
        return Ok(ApiResponse<List<ShowtimeDto>>.SuccessResponse(result));
    }

    [HttpGet("screen/{screenId:guid}")]
    public async Task<IActionResult> GetByScreen(Guid screenId, [FromQuery] DateTime? date = null)
    {
        var result = await _mediator.Send(new GetShowtimesByScreenQuery(screenId, date));
        return Ok(ApiResponse<List<ShowtimeDto>>.SuccessResponse(result));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateShowtimeDto dto)
    {
        var command = new CreateShowtimeCommand(dto.MovieId, dto.ScreenId, dto.ShowFormatId, dto.LanguageId, dto.StartTime);
        var result = await _mediator.Send(command);
        return Ok(ApiResponse<ShowtimeDto>.SuccessResponse(result, "Showtime created with seat availability."));
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateShowtimeDto dto)
    {
        var command = new UpdateShowtimeCommand(id, dto.ShowFormatId, dto.LanguageId, dto.StartTime, dto.Status);
        var result = await _mediator.Send(command);
        return Ok(ApiResponse<ShowtimeDto>.SuccessResponse(result, "Showtime updated."));
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Cancel(Guid id)
    {
        await _mediator.Send(new DeleteShowtimeCommand(id));
        return Ok(ApiResponse<bool>.SuccessResponse(true, "Showtime cancelled."));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("{id:guid}/pricing")]
    public async Task<IActionResult> SetPricing(Guid id, [FromBody] SetShowtimePricingDto dto)
    {
        var result = await _mediator.Send(new SetShowtimePricingCommand(id, dto.SeatCategoryId, dto.Price));
        return Ok(ApiResponse<ShowtimePricingDto>.SuccessResponse(result, "Pricing set."));
    }
}
