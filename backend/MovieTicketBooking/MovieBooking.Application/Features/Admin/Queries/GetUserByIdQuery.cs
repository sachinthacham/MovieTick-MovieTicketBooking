using MediatR;
using MovieBooking.Application.DTOs.Admin;

namespace MovieBooking.Application.Features.Admin.Queries;

public record GetUserByIdQuery(Guid UserId) : IRequest<UserAdminDto>;
