namespace MovieBooking.Application.DTOs.Theaters;

public class TheaterDto
{
    public Guid Id { get; set; }
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
    public bool IsActive { get; set; }
    public int TotalScreens { get; set; }
    public int TotalRatings { get; set; }
    public decimal? AverageRating { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<TheaterFacilityDto> Facilities { get; set; } = [];
    public List<TheaterImageDto> Images { get; set; } = [];
}

public class TheaterFacilityDto
{
    public Guid Id { get; set; }
    public string FacilityName { get; set; } = default!;
    public string? Icon { get; set; }
}

public class TheaterImageDto
{
    public Guid Id { get; set; }
    public string ImageUrl { get; set; } = default!;
    public bool IsPrimary { get; set; }
}

public class TheaterRatingDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; } = default!;
    public decimal Rating { get; set; }
    public string? Comment { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateTheaterDto
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
}

public class UpdateTheaterDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public bool? IsActive { get; set; }
}

public class AddFacilityDto
{
    public string FacilityName { get; set; } = default!;
    public string? Icon { get; set; }
}

public class RateTheaterDto
{
    public decimal Rating { get; set; }
    public string? Comment { get; set; }
}
