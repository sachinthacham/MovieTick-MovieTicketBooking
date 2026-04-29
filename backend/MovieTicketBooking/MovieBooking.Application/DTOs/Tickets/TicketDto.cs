namespace MovieBooking.Application.DTOs.Tickets;

public class TicketDto
{
    public Guid Id { get; set; }
    public Guid BookingId { get; set; }
    public string BookingReference { get; set; } = default!;
    public string TicketNumber { get; set; } = default!;
    public string QrCodeData { get; set; } = default!;
    public bool IsUsed { get; set; }
    public DateTime? UsedAt { get; set; }
    public string SeatNumber { get; set; } = default!;
    public string RowLabel { get; set; } = default!;
    public string SeatCategoryName { get; set; } = default!;
    public decimal Price { get; set; }
    public string MovieTitle { get; set; } = default!;
    public string TheaterName { get; set; } = default!;
    public string ScreenName { get; set; } = default!;
    public DateTime ShowtimeStart { get; set; }
}
