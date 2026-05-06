using MediatR;
using MovieBooking.Application.Features.Movies.Commands;
using MovieBooking.Application.Interfaces;

namespace MovieBooking.Application.Features.Movies.Handlers;

public class ToggleFeaturedHandler : IRequestHandler<ToggleFeaturedCommand, bool>
{
    private readonly IMovieRepository _movieRepository;

    public ToggleFeaturedHandler(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }

    public async Task<bool> Handle(ToggleFeaturedCommand request, CancellationToken cancellationToken)
    {
        var movie = await _movieRepository.GetByIdAsync(request.MovieId);
        if (movie == null)
            throw new KeyNotFoundException($"Movie with id {request.MovieId} not found.");

        movie.IsFeatured = request.IsFeatured;
        movie.UpdatedAt = DateTime.UtcNow;

        await _movieRepository.UpdateAsync(movie);
        return true;
    }
}

public class ToggleComingSoonHandler : IRequestHandler<ToggleComingSoonCommand, bool>
{
    private readonly IMovieRepository _movieRepository;

    public ToggleComingSoonHandler(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }

    public async Task<bool> Handle(ToggleComingSoonCommand request, CancellationToken cancellationToken)
    {
        var movie = await _movieRepository.GetByIdAsync(request.MovieId);
        if (movie == null)
            throw new KeyNotFoundException($"Movie with id {request.MovieId} not found.");

        movie.IsComingSoon = request.IsComingSoon;
        movie.UpdatedAt = DateTime.UtcNow;

        await _movieRepository.UpdateAsync(movie);
        return true;
    }
}
