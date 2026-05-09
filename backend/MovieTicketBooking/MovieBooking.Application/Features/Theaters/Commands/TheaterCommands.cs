using MediatR;
using Microsoft.AspNetCore.Http;
using MovieBooking.Application.DTOs.Theaters;

namespace MovieBooking.Application.Features.Theaters.Commands;

public record CreateTheaterCommand(
    string Name, string Description, string Address, string City,
    string State, string Country, string? PhoneNumber, string? Email,
    double Latitude, double Longitude) : IRequest<TheaterDto>;

public record UpdateTheaterCommand(
    Guid Id, string? Name, string? Description, string? Address,
    string? City, string? State, string? Country, string? PhoneNumber,
    string? Email, double? Latitude, double? Longitude, bool? IsActive) : IRequest<TheaterDto>;

public record DeleteTheaterCommand(Guid Id) : IRequest<bool>;

public record AddTheaterFacilityCommand(Guid TheaterId, string FacilityName, string? Icon) : IRequest<TheaterFacilityDto>;
public record RemoveTheaterFacilityCommand(Guid FacilityId) : IRequest<bool>;

public record UploadTheaterImageCommand(Guid TheaterId, IFormFile File, bool IsPrimary) : IRequest<TheaterImageDto>;
public record DeleteTheaterImageCommand(Guid ImageId) : IRequest<bool>;
public record SetPrimaryTheaterImageCommand(Guid TheaterId, Guid ImageId) : IRequest<bool>;

public record RateTheaterCommand(Guid TheaterId, Guid UserId, decimal Rating, string? Comment) : IRequest<bool>;
