using MediatR;
using MovieBooking.Application.DTOs.Movies;
using MovieBooking.Application.Features.Movies.Commands;
using MovieBooking.Application.Interfaces;
using MovieBooking.Domain.Entities;

namespace MovieBooking.Application.Features.Movies.Handlers;

public class UploadMoviePosterHandler : IRequestHandler<UploadMoviePosterCommand, MoviePosterDto>
{
    private readonly IMovieRepository _movieRepository;
    private readonly IMoviePosterRepository _posterRepository;
    private readonly IFileStorageService _fileStorage;

    public UploadMoviePosterHandler(
        IMovieRepository movieRepository,
        IMoviePosterRepository posterRepository,
        IFileStorageService fileStorage)
    {
        _movieRepository = movieRepository;
        _posterRepository = posterRepository;
        _fileStorage = fileStorage;
    }

    public async Task<MoviePosterDto> Handle(UploadMoviePosterCommand request, CancellationToken cancellationToken)
    {
        var movie = await _movieRepository.GetByIdAsync(request.MovieId);
        if (movie == null) throw new KeyNotFoundException("Movie not found.");

        if (request.IsPrimary)
            await _posterRepository.ClearPrimaryAsync(request.MovieId);

        var url = await _fileStorage.UploadAsync(request.File, "movie-posters");

        var poster = new MoviePoster
        {
            MovieId = request.MovieId,
            ImageUrl = url,
            IsPrimary = request.IsPrimary,
            PosterType = request.PosterType
        };

        await _posterRepository.AddAsync(poster);

        return new MoviePosterDto
        {
            Id = poster.Id,
            ImageUrl = poster.ImageUrl,
            IsPrimary = poster.IsPrimary,
            PosterType = poster.PosterType.ToString()
        };
    }
}

public class DeleteMoviePosterHandler : IRequestHandler<DeleteMoviePosterCommand, bool>
{
    private readonly IMoviePosterRepository _posterRepository;
    private readonly IFileStorageService _fileStorage;

    public DeleteMoviePosterHandler(IMoviePosterRepository posterRepository, IFileStorageService fileStorage)
    {
        _posterRepository = posterRepository;
        _fileStorage = fileStorage;
    }

    public async Task<bool> Handle(DeleteMoviePosterCommand request, CancellationToken cancellationToken)
    {
        var poster = await _posterRepository.GetByIdAsync(request.PosterId);
        if (poster == null) throw new KeyNotFoundException("Poster not found.");

        await _fileStorage.DeleteAsync(poster.ImageUrl);
        await _posterRepository.DeleteAsync(poster);
        return true;
    }
}

public class SetPrimaryPosterHandler : IRequestHandler<SetPrimaryPosterCommand, bool>
{
    private readonly IMoviePosterRepository _posterRepository;

    public SetPrimaryPosterHandler(IMoviePosterRepository posterRepository)
    {
        _posterRepository = posterRepository;
    }

    public async Task<bool> Handle(SetPrimaryPosterCommand request, CancellationToken cancellationToken)
    {
        await _posterRepository.ClearPrimaryAsync(request.MovieId);

        var poster = await _posterRepository.GetByIdAsync(request.PosterId);
        if (poster == null) throw new KeyNotFoundException("Poster not found.");

        poster.IsPrimary = true;
        await _posterRepository.UpdateAsync(poster);
        return true;
    }
}
