namespace MovieBooking.Application.DTOs.Notifications;

public class NotificationDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string Message { get; set; } = default!;
    public string Type { get; set; } = default!;
    public bool IsRead { get; set; }
    public string? MetaData { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class PagedNotificationsDto
{
    public List<NotificationDto> Items { get; set; } = [];
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int UnreadCount { get; set; }
}
