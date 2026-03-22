using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieBooking.Application.DTOs.Auth;
using MovieBooking.Application.Features.Auth.Commands;

namespace MovieBooking.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserDto dto)
    {
        var command = new RegisterUserCommand(dto.FullName, dto.Email, dto.Password);
        var userId = await _mediator.Send(command);

        return Ok(userId);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var command = new LoginUserCommand(dto.Email, dto.Password);
        var token = await _mediator.Send(command);

        return Ok(new { token });
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult GetProfile()
    {
        return Ok("You are authorized");
    }
}
