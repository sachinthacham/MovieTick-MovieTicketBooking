using MovieBooking.Domain.Common;

namespace MovieBooking.Domain.Entities;

public class RefreshToken : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;
    public string Token { get; set; } = default!;
    public DateTime ExpiryDate { get; set; }
    public bool IsRevoked { get; set; }
}