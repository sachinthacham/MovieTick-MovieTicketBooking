namespace MovieBooking.Application.DTOs.Showtimes;

public class ShowFormatDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
}

public class CreateShowFormatDto
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
}

public class ShowtimeDto
{
    public Guid Id { get; set; }
    public Guid MovieId { get; set; }
    public string MovieTitle { get; set; } = default!;
    public Guid ScreenId { get; set; }
    public string ScreenName { get; set; } = default!;
    public string TheaterName { get; set; } = default!;
    public string ShowFormatName { get; set; } = default!;
    public string LanguageName { get; set; } = default!;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Status { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public List<ShowtimePricingDto> Pricings { get; set; } = [];
}

public class ShowtimePricingDto
{
    public Guid Id { get; set; }
    public Guid SeatCategoryId { get; set; }
    public string SeatCategoryName { get; set; } = default!;
    public decimal Price { get; set; }
}

public class CreateShowtimeDto
{
    public Guid MovieId { get; set; }
    public Guid ScreenId { get; set; }
    public Guid ShowFormatId { get; set; }
    public Guid LanguageId { get; set; }
    public DateTime StartTime { get; set; }
}

public class UpdateShowtimeDto
{
    public Guid? ShowFormatId { get; set; }
    public Guid? LanguageId { get; set; }
    public DateTime? StartTime { get; set; }
    public string? Status { get; set; }
}

public class SetShowtimePricingDto
{
    public Guid SeatCategoryId { get; set; }
    public decimal Price { get; set; }
}
