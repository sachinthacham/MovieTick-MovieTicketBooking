using MediatR;
using MovieBooking.Application.Features.Notifications.Commands;
using MovieBooking.Application.Interfaces;

namespace MovieBooking.Application.Features.Notifications.Handlers;

public class MarkNotificationsReadHandler : IRequestHandler<MarkNotificationsReadCommand, bool>
{
    private readonly INotificationRepository _notificationRepository;

    public MarkNotificationsReadHandler(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task<bool> Handle(MarkNotificationsReadCommand request, CancellationToken cancellationToken)
    {
        await _notificationRepository.MarkAllReadAsync(request.UserId, request.NotificationIds);
        return true;
    }
}
