using MediatR;
using MovieBooking.Application.DTOs.Notifications;

namespace MovieBooking.Application.Features.Notifications.Queries;

public record GetUserNotificationsQuery(Guid UserId, int Page = 1, int PageSize = 20) : IRequest<PagedNotificationsDto>;
