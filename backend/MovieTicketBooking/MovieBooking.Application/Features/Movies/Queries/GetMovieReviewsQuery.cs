using MediatR;
using MovieBooking.Application.DTOs.Movies;

namespace MovieBooking.Application.Features.Movies.Queries;

public record GetMovieReviewsQuery(Guid MovieId, bool ApprovedOnly = true) : IRequest<List<MovieReviewDto>>;
