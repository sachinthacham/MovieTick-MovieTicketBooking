using MovieBooking.Domain.Common;

namespace MovieBooking.Domain.Entities;

public class Notification : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    public string Title { get; set; } = default!;
    public string Message { get; set; } = default!;
    public string Type { get; set; } = default!;
    public bool IsRead { get; set; }
    public string? MetaData { get; set; }
}
