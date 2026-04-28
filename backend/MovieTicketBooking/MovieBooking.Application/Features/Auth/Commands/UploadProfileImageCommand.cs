using MediatR;
using Microsoft.AspNetCore.Http;

namespace MovieBooking.Application.Features.Auth.Commands;

public record UploadProfileImageCommand(Guid UserId, IFormFile File) : IRequest<string>;
