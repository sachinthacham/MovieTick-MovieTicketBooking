using MediatR;

namespace MovieBooking.Application.Features.Theaters.Queries;

public record GetDistinctCitiesQuery : IRequest<List<string>>;
