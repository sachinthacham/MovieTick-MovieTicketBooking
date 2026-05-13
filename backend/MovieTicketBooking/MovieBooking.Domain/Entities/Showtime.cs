using MovieBooking.Domain.Common;
using MovieBooking.Domain.Enums;

namespace MovieBooking.Domain.Entities;

public class Showtime : BaseEntity
{
    public Guid MovieId { get; set; }
    public Movie Movie { get; set; } = default!;

    public Guid ScreenId { get; set; }
    public Screen Screen { get; set; } = default!;

    public Guid ShowFormatId { get; set; }
    public ShowFormat ShowFormat { get; set; } = default!;

    public Guid LanguageId { get; set; }
    public Language Language { get; set; } = default!;

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public ShowtimeStatus Status { get; set; } = ShowtimeStatus.Scheduled;

    public ICollection<ShowtimePricing> Pricings { get; set; } = [];
    public ICollection<ShowtimeSeat> ShowtimeSeats { get; set; } = [];
}
