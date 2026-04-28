using MediatR;

namespace MovieBooking.Application.Features.Auth.Commands;

public record ResetPasswordCommand(string Email, string Token, string NewPassword) : IRequest<bool>;
