using MovieBooking.Domain.Entities;

namespace MovieBooking.Application.Interfaces;

public interface INotificationRepository
{
    Task AddAsync(Notification notification);
    Task<(List<Notification> Items, int TotalCount)> GetByUserAsync(Guid userId, int page, int pageSize);
    Task UpdateAsync(Notification notification);
    Task MarkAllReadAsync(Guid userId, IEnumerable<Guid> notificationIds);
}
