using MediatR;
using Microsoft.AspNetCore.Http;
using MovieBooking.Application.DTOs.Movies;
using MovieBooking.Domain.Enums;

namespace MovieBooking.Application.Features.Movies.Commands;

public record UploadMoviePosterCommand(Guid MovieId, IFormFile File, PosterType PosterType, bool IsPrimary) : IRequest<MoviePosterDto>;
public record DeleteMoviePosterCommand(Guid PosterId) : IRequest<bool>;
public record SetPrimaryPosterCommand(Guid MovieId, Guid PosterId) : IRequest<bool>;
