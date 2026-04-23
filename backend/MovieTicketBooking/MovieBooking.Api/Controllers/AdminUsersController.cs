using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieBooking.Application.Common.Responses;
using MovieBooking.Application.DTOs.Admin;
using MovieBooking.Application.Features.Admin.Commands;
using MovieBooking.Application.Features.Admin.Queries;

namespace MovieBooking.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/admin/users")]
[ApiVersion("1.0")]
[Authorize(Roles = "Admin")]
public class AdminUsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminUsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        [FromQuery] string? role = null)
    {
        var result = await _mediator.Send(new GetAllUsersQuery(page, pageSize, search, role));
        return Ok(ApiResponse<PagedUsersDto>.SuccessResponse(result));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetUserByIdQuery(id));
        return Ok(ApiResponse<UserAdminDto>.SuccessResponse(result));
    }

    [HttpPatch("{id:guid}/role")]
    public async Task<IActionResult> UpdateRole(Guid id, [FromBody] UpdateRoleDto dto)
    {
        var result = await _mediator.Send(new UpdateUserRoleCommand(id, dto.Role));
        return Ok(ApiResponse<bool>.SuccessResponse(result, "User role updated."));
    }

    [HttpPatch("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        var result = await _mediator.Send(new DeactivateUserCommand(id));
        return Ok(ApiResponse<bool>.SuccessResponse(result, "User deactivated."));
    }

    [HttpPatch("{id:guid}/activate")]
    public async Task<IActionResult> Activate(Guid id)
    {
        var result = await _mediator.Send(new ActivateUserCommand(id));
        return Ok(ApiResponse<bool>.SuccessResponse(result, "User activated."));
    }
}

public class UpdateRoleDto
{
    public string Role { get; set; } = default!;
}
