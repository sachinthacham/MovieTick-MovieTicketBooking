using MediatR;

namespace MovieBooking.Application.Features.Notifications.Commands;

public record MarkNotificationsReadCommand(Guid UserId, List<Guid> NotificationIds) : IRequest<bool>;
