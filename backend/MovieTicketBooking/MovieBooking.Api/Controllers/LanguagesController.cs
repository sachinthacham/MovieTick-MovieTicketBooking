using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieBooking.Application.Common.Responses;
using MovieBooking.Application.DTOs.Languages;
using MovieBooking.Application.Features.Languages.Commands;
using MovieBooking.Application.Features.Languages.Queries;

namespace MovieBooking.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/languages")]
public class LanguagesController : ControllerBase
{
    private readonly IMediator _mediator;

    public LanguagesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllLanguagesQuery());
        return Ok(ApiResponse<List<LanguageDto>>.SuccessResponse(result));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetLanguageByIdQuery(id));
        return Ok(ApiResponse<LanguageDto>.SuccessResponse(result));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateLanguageDto dto)
    {
        var result = await _mediator.Send(new CreateLanguageCommand(dto.Name, dto.Code));
        return Ok(ApiResponse<LanguageDto>.SuccessResponse(result, "Language created."));
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateLanguageDto dto)
    {
        var result = await _mediator.Send(new UpdateLanguageCommand(id, dto.Name, dto.Code));
        return Ok(ApiResponse<LanguageDto>.SuccessResponse(result, "Language updated."));
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteLanguageCommand(id));
        return Ok(ApiResponse<bool>.SuccessResponse(true, "Language deleted."));
    }
}
