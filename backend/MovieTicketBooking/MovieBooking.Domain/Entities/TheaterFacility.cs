using MovieBooking.Domain.Common;

namespace MovieBooking.Domain.Entities;

public class TheaterFacility : BaseEntity
{
    public Guid TheaterId { get; set; }
    public Theater Theater { get; set; } = default!;

    public string FacilityName { get; set; } = default!;
    public string? Icon { get; set; }
}
