using MediatR;
using MovieBooking.Application.Features.Movies.Commands;
using MovieBooking.Application.Interfaces;
using MovieBooking.Domain.Entities;

namespace MovieBooking.Application.Features.Movies.Handlers;

public class AddMovieGenreHandler : IRequestHandler<AddMovieGenreCommand, bool>
{
    private readonly IMovieRepository _movieRepository;
    private readonly IGenreRepository _genreRepository;

    public AddMovieGenreHandler(IMovieRepository movieRepository, IGenreRepository genreRepository)
    {
        _movieRepository = movieRepository;
        _genreRepository = genreRepository;
    }

    public async Task<bool> Handle(AddMovieGenreCommand request, CancellationToken cancellationToken)
    {
        var movie = await _movieRepository.GetByIdAsync(request.MovieId);
        if (movie == null) throw new KeyNotFoundException("Movie not found.");

        var genre = await _genreRepository.GetByIdAsync(request.GenreId);
        if (genre == null) throw new KeyNotFoundException("Genre not found.");

        if (movie.Genres.Any(g => g.GenreId == request.GenreId))
            return true;

        movie.Genres.Add(new MovieGenre { MovieId = request.MovieId, GenreId = request.GenreId });
        movie.UpdatedAt = DateTime.UtcNow;
        await _movieRepository.UpdateAsync(movie);
        return true;
    }
}

public class RemoveMovieGenreHandler : IRequestHandler<RemoveMovieGenreCommand, bool>
{
    private readonly IMovieRepository _movieRepository;

    public RemoveMovieGenreHandler(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }

    public async Task<bool> Handle(RemoveMovieGenreCommand request, CancellationToken cancellationToken)
    {
        var movie = await _movieRepository.GetByIdAsync(request.MovieId);
        if (movie == null) throw new KeyNotFoundException("Movie not found.");

        var mg = movie.Genres.FirstOrDefault(g => g.GenreId == request.GenreId);
        if (mg == null) return true;

        movie.Genres.Remove(mg);
        movie.UpdatedAt = DateTime.UtcNow;
        await _movieRepository.UpdateAsync(movie);
        return true;
    }
}
