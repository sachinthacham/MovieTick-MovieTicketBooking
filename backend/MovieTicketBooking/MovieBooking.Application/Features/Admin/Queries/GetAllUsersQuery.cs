using MediatR;
using MovieBooking.Application.DTOs.Admin;

namespace MovieBooking.Application.Features.Admin.Queries;

public record GetAllUsersQuery(
    int Page = 1,
    int PageSize = 10,
    string? Search = null,
    string? Role = null
) : IRequest<PagedUsersDto>;
