namespace MovieBooking.Application.DTOs.Admin;

public class UserAdminDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Role { get; set; } = default!;
    public string? PhoneNumber { get; set; }
    public string? ProfileImageUrl { get; set; }
    public bool IsActive { get; set; }
    public string? SocialProvider { get; set; }
    public DateTime CreatedAt { get; set; }
    public int TotalBookings { get; set; }
    public decimal TotalSpent { get; set; }
}

public class PagedUsersDto
{
    public List<UserAdminDto> Items { get; set; } = [];
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}
