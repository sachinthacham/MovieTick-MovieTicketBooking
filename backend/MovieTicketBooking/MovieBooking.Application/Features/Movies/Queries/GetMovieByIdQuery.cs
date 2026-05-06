using MediatR;
using MovieBooking.Application.DTOs.Movies;

namespace MovieBooking.Application.Features.Movies.Queries;

public record GetMovieByIdQuery(Guid Id) : IRequest<MovieDto>;
