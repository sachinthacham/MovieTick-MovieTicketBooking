using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieBooking.Application.Common.Responses;
using MovieBooking.Application.DTOs.Genres;
using MovieBooking.Application.Features.Genres.Commands;
using MovieBooking.Application.Features.Genres.Queries;

namespace MovieBooking.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/genres")]
public class GenresController : ControllerBase
{
    private readonly IMediator _mediator;

    public GenresController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllGenresQuery());
        return Ok(ApiResponse<List<GenreDto>>.SuccessResponse(result));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetGenreByIdQuery(id));
        return Ok(ApiResponse<GenreDto>.SuccessResponse(result));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateGenreDto dto)
    {
        var result = await _mediator.Send(new CreateGenreCommand(dto.Name));
        return Ok(ApiResponse<GenreDto>.SuccessResponse(result, "Genre created."));
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateGenreDto dto)
    {
        var result = await _mediator.Send(new UpdateGenreCommand(id, dto.Name));
        return Ok(ApiResponse<GenreDto>.SuccessResponse(result, "Genre updated."));
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteGenreCommand(id));
        return Ok(ApiResponse<bool>.SuccessResponse(true, "Genre deleted."));
    }
}
