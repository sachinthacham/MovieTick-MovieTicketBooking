using MediatR;
using MovieBooking.Application.DTOs.Notifications;
using MovieBooking.Application.Features.Notifications.Queries;
using MovieBooking.Application.Interfaces;

namespace MovieBooking.Application.Features.Notifications.Handlers;

public class GetUserNotificationsHandler : IRequestHandler<GetUserNotificationsQuery, PagedNotificationsDto>
{
    private readonly INotificationRepository _notificationRepository;

    public GetUserNotificationsHandler(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task<PagedNotificationsDto> Handle(GetUserNotificationsQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _notificationRepository.GetByUserAsync(request.UserId, request.Page, request.PageSize);

        var dtos = items.Select(n => new NotificationDto
        {
            Id = n.Id,
            Title = n.Title,
            Message = n.Message,
            Type = n.Type,
            IsRead = n.IsRead,
            MetaData = n.MetaData,
            CreatedAt = n.CreatedAt
        }).ToList();

        return new PagedNotificationsDto
        {
            Items = dtos,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize,
            UnreadCount = dtos.Count(n => !n.IsRead)
        };
    }
}
