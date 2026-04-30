using MediatR;
using MovieBooking.Application.DTOs.Seats;

namespace MovieBooking.Application.Features.SeatCategories.Commands;

public record CreateSeatCategoryCommand(string Name, decimal DefaultPrice, string Color, string? Description) : IRequest<SeatCategoryDto>;
public record UpdateSeatCategoryCommand(Guid Id, string? Name, decimal? DefaultPrice, string? Color, string? Description) : IRequest<SeatCategoryDto>;
public record DeleteSeatCategoryCommand(Guid Id) : IRequest<bool>;
