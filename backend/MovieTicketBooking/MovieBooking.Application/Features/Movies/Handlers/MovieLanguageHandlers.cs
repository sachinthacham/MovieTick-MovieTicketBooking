using MediatR;
using MovieBooking.Application.Features.Movies.Commands;
using MovieBooking.Application.Interfaces;
using MovieBooking.Domain.Entities;

namespace MovieBooking.Application.Features.Movies.Handlers;

public class AddMovieLanguageHandler : IRequestHandler<AddMovieLanguageCommand, bool>
{
    private readonly IMovieRepository _movieRepository;
    private readonly ILanguageRepository _languageRepository;

    public AddMovieLanguageHandler(IMovieRepository movieRepository, ILanguageRepository languageRepository)
    {
        _movieRepository = movieRepository;
        _languageRepository = languageRepository;
    }

    public async Task<bool> Handle(AddMovieLanguageCommand request, CancellationToken cancellationToken)
    {
        var movie = await _movieRepository.GetByIdAsync(request.MovieId);
        if (movie == null) throw new KeyNotFoundException("Movie not found.");

        var language = await _languageRepository.GetByIdAsync(request.LanguageId);
        if (language == null) throw new KeyNotFoundException("Language not found.");

        if (movie.Languages.Any(l => l.LanguageId == request.LanguageId))
            return true;

        movie.Languages.Add(new MovieLanguage { MovieId = request.MovieId, LanguageId = request.LanguageId });
        movie.UpdatedAt = DateTime.UtcNow;
        await _movieRepository.UpdateAsync(movie);
        return true;
    }
}

public class RemoveMovieLanguageHandler : IRequestHandler<RemoveMovieLanguageCommand, bool>
{
    private readonly IMovieRepository _movieRepository;

    public RemoveMovieLanguageHandler(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }

    public async Task<bool> Handle(RemoveMovieLanguageCommand request, CancellationToken cancellationToken)
    {
        var movie = await _movieRepository.GetByIdAsync(request.MovieId);
        if (movie == null) throw new KeyNotFoundException("Movie not found.");

        var ml = movie.Languages.FirstOrDefault(l => l.LanguageId == request.LanguageId);
        if (ml == null) return true;

        movie.Languages.Remove(ml);
        movie.UpdatedAt = DateTime.UtcNow;
        await _movieRepository.UpdateAsync(movie);
        return true;
    }
}
