namespace MovieBooking.Application.DTOs.Seats;

public class SeatDto
{
    public Guid Id { get; set; }
    public Guid ScreenId { get; set; }
    public Guid SeatCategoryId { get; set; }
    public string SeatCategoryName { get; set; } = default!;
    public string SeatNumber { get; set; } = default!;
    public string Row { get; set; } = default!;
    public int Column { get; set; }
    public bool IsBlocked { get; set; }
    public bool IsActive { get; set; }
}

public class SeatCategoryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public decimal DefaultPrice { get; set; }
    public string Color { get; set; } = default!;
    public string? Description { get; set; }
}

public class CreateSeatCategoryDto
{
    public string Name { get; set; } = default!;
    public decimal DefaultPrice { get; set; }
    public string Color { get; set; } = "#000000";
    public string? Description { get; set; }
}

public class UpdateSeatCategoryDto
{
    public string? Name { get; set; }
    public decimal? DefaultPrice { get; set; }
    public string? Color { get; set; }
    public string? Description { get; set; }
}

public class BulkCreateSeatsDto
{
    public Guid ScreenId { get; set; }
    public List<SeatRowConfig> Rows { get; set; } = [];
}

public class SeatRowConfig
{
    public string RowLabel { get; set; } = default!;
    public int NumberOfSeats { get; set; }
    public Guid SeatCategoryId { get; set; }
}

public class UpdateSeatDto
{
    public Guid? SeatCategoryId { get; set; }
    public bool? IsActive { get; set; }
}

public class SeatAvailabilityDto
{
    /// <summary>ShowtimeSeat row id — use for lock/booking APIs.</summary>
    public Guid Id { get; set; }
    public Guid SeatId { get; set; }
    public Guid SeatCategoryId { get; set; }
    public string SeatNumber { get; set; } = default!;
    public string Row { get; set; } = default!;
    public int Column { get; set; }
    public string SeatCategoryName { get; set; } = default!;
    public string Color { get; set; } = default!;
    public string Status { get; set; } = default!;
    public decimal? Price { get; set; }
}
