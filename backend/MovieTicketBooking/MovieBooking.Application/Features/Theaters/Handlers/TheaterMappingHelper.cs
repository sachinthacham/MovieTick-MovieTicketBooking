using MovieBooking.Application.DTOs.Theaters;
using MovieBooking.Domain.Entities;

namespace MovieBooking.Application.Features.Theaters.Handlers;

internal static class TheaterMappingHelper
{
    internal static TheaterDto ToDto(Theater theater)
    {
        var ratings = theater.Ratings?.ToList() ?? [];
        decimal? avg = ratings.Count == 0
            ? null
            : Math.Round((decimal)ratings.Average(r => (double)r.Rating), 1, MidpointRounding.AwayFromZero);

        return new TheaterDto
        {
        Id = theater.Id,
        Name = theater.Name,
        Description = theater.Description,
        Address = theater.Address,
        City = theater.City,
        State = theater.State,
        Country = theater.Country,
        PhoneNumber = theater.PhoneNumber,
        Email = theater.Email,
        Latitude = theater.Latitude,
        Longitude = theater.Longitude,
        IsActive = theater.IsActive,
        TotalScreens = theater.Screens?.Count ?? 0,
        TotalRatings = ratings.Count,
        AverageRating = avg,
        CreatedAt = theater.CreatedAt,
        Facilities = theater.Facilities.Select(f => new TheaterFacilityDto
        {
            Id = f.Id,
            FacilityName = f.FacilityName,
            Icon = f.Icon
        }).ToList(),
        Images = theater.Images.Select(i => new TheaterImageDto
        {
            Id = i.Id,
            ImageUrl = i.ImageUrl,
            IsPrimary = i.IsPrimary
        }).ToList()
        };
    }
}
