using MovieBooking.Domain.Common;

namespace MovieBooking.Domain.Entities;

public class Theater : BaseEntity
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string City { get; set; } = default!;
    public string State { get; set; } = default!;
    public string Country { get; set; } = default!;
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<Screen> Screens { get; set; } = [];
    public ICollection<TheaterFacility> Facilities { get; set; } = [];
    public ICollection<TheaterImage> Images { get; set; } = [];
    public ICollection<TheaterRating> Ratings { get; set; } = [];
}
