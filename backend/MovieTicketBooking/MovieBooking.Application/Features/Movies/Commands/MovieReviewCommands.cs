using MediatR;
using MovieBooking.Application.DTOs.Movies;

namespace MovieBooking.Application.Features.Movies.Commands;

public record AddMovieReviewCommand(Guid MovieId, Guid UserId, string ReviewText) : IRequest<MovieReviewDto>;
public record UpdateMovieReviewCommand(Guid ReviewId, Guid UserId, string ReviewText) : IRequest<MovieReviewDto>;
public record DeleteMovieReviewCommand(Guid ReviewId, Guid UserId, bool IsAdmin) : IRequest<bool>;
public record ApproveMovieReviewCommand(Guid ReviewId) : IRequest<bool>;
