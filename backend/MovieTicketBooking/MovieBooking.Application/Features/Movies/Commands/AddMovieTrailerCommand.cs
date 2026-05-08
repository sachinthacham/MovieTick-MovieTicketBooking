using MediatR;
using MovieBooking.Application.DTOs.Movies;

namespace MovieBooking.Application.Features.Movies.Commands;

public record AddMovieTrailerCommand(Guid MovieId, string Title, string Url, bool IsPrimary) : IRequest<MovieTrailerDto>;
public record DeleteMovieTrailerCommand(Guid TrailerId) : IRequest<bool>;
