namespace MovieBooking.Application.DTOs.Screens;

public class ScreenDto
{
    public Guid Id { get; set; }
    public Guid TheaterId { get; set; }
    public string Name { get; set; } = default!;
    public int TotalSeats { get; set; }
    public string ScreenType { get; set; } = default!;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateScreenDto
{
    public Guid TheaterId { get; set; }
    public string Name { get; set; } = default!;
    public string ScreenType { get; set; } = "Standard";
}

public class UpdateScreenDto
{
    public string? Name { get; set; }
    public string? ScreenType { get; set; }
    public bool? IsActive { get; set; }
}
