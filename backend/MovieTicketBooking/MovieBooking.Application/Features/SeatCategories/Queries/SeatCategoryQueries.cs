using MediatR;
using MovieBooking.Application.DTOs.Seats;

namespace MovieBooking.Application.Features.SeatCategories.Queries;

public record GetAllSeatCategoriesQuery : IRequest<List<SeatCategoryDto>>;
public record GetSeatCategoryByIdQuery(Guid Id) : IRequest<SeatCategoryDto>;
