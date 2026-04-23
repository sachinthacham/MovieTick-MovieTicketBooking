using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieBooking.Application.Common.Responses;
using MovieBooking.Application.DTOs.Seats;
using MovieBooking.Application.Features.SeatCategories.Commands;
using MovieBooking.Application.Features.SeatCategories.Queries;

namespace MovieBooking.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/seat-categories")]
public class SeatCategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public SeatCategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllSeatCategoriesQuery());
        return Ok(ApiResponse<List<SeatCategoryDto>>.SuccessResponse(result));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetSeatCategoryByIdQuery(id));
        return Ok(ApiResponse<SeatCategoryDto>.SuccessResponse(result));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSeatCategoryDto dto)
    {
        var result = await _mediator.Send(new CreateSeatCategoryCommand(dto.Name, dto.DefaultPrice, dto.Color, dto.Description));
        return Ok(ApiResponse<SeatCategoryDto>.SuccessResponse(result, "Seat category created."));
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSeatCategoryDto dto)
    {
        var result = await _mediator.Send(new UpdateSeatCategoryCommand(id, dto.Name, dto.DefaultPrice, dto.Color, dto.Description));
        return Ok(ApiResponse<SeatCategoryDto>.SuccessResponse(result, "Seat category updated."));
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteSeatCategoryCommand(id));
        return Ok(ApiResponse<bool>.SuccessResponse(true, "Seat category deleted."));
    }
}
