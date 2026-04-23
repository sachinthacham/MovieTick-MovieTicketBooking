using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieBooking.Application.Common.Responses;
using MovieBooking.Application.DTOs.Theaters;
using MovieBooking.Application.Features.Theaters.Commands;
using MovieBooking.Application.Features.Theaters.Queries;
using System.Security.Claims;

namespace MovieBooking.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/theaters")]
public class TheatersController : ControllerBase
{
    private readonly IMediator _mediator;

    public TheatersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? city = null, [FromQuery] bool? isActive = null)
    {
        var result = await _mediator.Send(new GetAllTheatersQuery(city, isActive));
        return Ok(ApiResponse<List<TheaterDto>>.SuccessResponse(result));
    }

    [HttpGet("cities")]
    public async Task<IActionResult> GetCities()
    {
        var result = await _mediator.Send(new GetDistinctCitiesQuery());
        return Ok(ApiResponse<List<string>>.SuccessResponse(result));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetTheaterByIdQuery(id));
        return Ok(ApiResponse<TheaterDto>.SuccessResponse(result));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTheaterDto dto)
    {
        var command = new CreateTheaterCommand(dto.Name, dto.Description, dto.Address, dto.City, dto.State, dto.Country, dto.PhoneNumber, dto.Email, dto.Latitude, dto.Longitude);
        var result = await _mediator.Send(command);
        return Ok(ApiResponse<TheaterDto>.SuccessResponse(result, "Theater created."));
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTheaterDto dto)
    {
        var command = new UpdateTheaterCommand(id, dto.Name, dto.Description, dto.Address, dto.City, dto.State, dto.Country, dto.PhoneNumber, dto.Email, dto.Latitude, dto.Longitude, dto.IsActive);
        var result = await _mediator.Send(command);
        return Ok(ApiResponse<TheaterDto>.SuccessResponse(result, "Theater updated."));
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteTheaterCommand(id));
        return Ok(ApiResponse<bool>.SuccessResponse(true, "Theater deleted."));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("{id:guid}/facilities")]
    public async Task<IActionResult> AddFacility(Guid id, [FromBody] AddFacilityDto dto)
    {
        var result = await _mediator.Send(new AddTheaterFacilityCommand(id, dto.FacilityName, dto.Icon));
        return Ok(ApiResponse<TheaterFacilityDto>.SuccessResponse(result, "Facility added."));
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("facilities/{facilityId:guid}")]
    public async Task<IActionResult> RemoveFacility(Guid facilityId)
    {
        await _mediator.Send(new RemoveTheaterFacilityCommand(facilityId));
        return Ok(ApiResponse<bool>.SuccessResponse(true, "Facility removed."));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("{id:guid}/images")]
    public async Task<IActionResult> UploadImage(Guid id, IFormFile file, [FromQuery] bool isPrimary = false)
    {
        var result = await _mediator.Send(new UploadTheaterImageCommand(id, file, isPrimary));
        return Ok(ApiResponse<TheaterImageDto>.SuccessResponse(result, "Image uploaded."));
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("images/{imageId:guid}")]
    public async Task<IActionResult> DeleteImage(Guid imageId)
    {
        await _mediator.Send(new DeleteTheaterImageCommand(imageId));
        return Ok(ApiResponse<bool>.SuccessResponse(true, "Image deleted."));
    }

    [Authorize(Roles = "Admin")]
    [HttpPatch("{id:guid}/images/{imageId:guid}/primary")]
    public async Task<IActionResult> SetPrimaryImage(Guid id, Guid imageId)
    {
        await _mediator.Send(new SetPrimaryTheaterImageCommand(id, imageId));
        return Ok(ApiResponse<bool>.SuccessResponse(true, "Primary image set."));
    }

    [HttpGet("{id:guid}/ratings")]
    public async Task<IActionResult> GetRatings(Guid id)
    {
        var result = await _mediator.Send(new GetTheaterRatingsQuery(id));
        return Ok(ApiResponse<List<TheaterRatingDto>>.SuccessResponse(result));
    }

    [Authorize]
    [HttpPost("{id:guid}/rate")]
    public async Task<IActionResult> Rate(Guid id, [FromBody] RateTheaterDto dto)
    {
        var userId = GetCurrentUserId();
        await _mediator.Send(new RateTheaterCommand(id, userId, dto.Rating, dto.Comment));
        return Ok(ApiResponse<bool>.SuccessResponse(true, "Rating submitted."));
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            throw new UnauthorizedAccessException("Invalid token.");
        return userId;
    }
}
