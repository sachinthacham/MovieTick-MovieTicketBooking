using MediatR;
using MovieBooking.Application.DTOs.Movies;
using MovieBooking.Application.Features.Movies.Commands;
using MovieBooking.Application.Interfaces;

namespace MovieBooking.Application.Features.Movies.Handlers;

public class UpdateMovieHandler : IRequestHandler<UpdateMovieCommand, MovieDto>
{
    private readonly IMovieRepository _movieRepository;

    public UpdateMovieHandler(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }

    public async Task<MovieDto> Handle(UpdateMovieCommand request, CancellationToken cancellationToken)
    {
        var movie = await _movieRepository.GetByIdAsync(request.Id);
        if (movie == null)
            throw new KeyNotFoundException($"Movie with id {request.Id} not found.");

        if (!string.IsNullOrWhiteSpace(request.Title)) movie.Title = request.Title;
        if (!string.IsNullOrWhiteSpace(request.Description)) movie.Description = request.Description;
        if (request.DurationMinutes.HasValue) movie.DurationMinutes = request.DurationMinutes.Value;
        if (request.ReleaseDate.HasValue) movie.ReleaseDate = request.ReleaseDate.Value;
        if (!string.IsNullOrWhiteSpace(request.Director)) movie.Director = request.Director;
        if (!string.IsNullOrWhiteSpace(request.Cast)) movie.Cast = request.Cast;
        if (!string.IsNullOrWhiteSpace(request.CertificateRating)) movie.CertificateRating = request.CertificateRating;
        if (request.IsActive.HasValue) movie.IsActive = request.IsActive.Value;
        movie.UpdatedAt = DateTime.UtcNow;

        await _movieRepository.UpdateAsync(movie);

        return MovieMappingHelper.ToDto(movie);
    }
}
