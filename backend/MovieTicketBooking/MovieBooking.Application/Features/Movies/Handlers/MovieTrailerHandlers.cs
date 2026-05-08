using MediatR;
using MovieBooking.Application.DTOs.Movies;
using MovieBooking.Application.Features.Movies.Commands;
using MovieBooking.Application.Interfaces;
using MovieBooking.Domain.Entities;

namespace MovieBooking.Application.Features.Movies.Handlers;

public class AddMovieTrailerHandler : IRequestHandler<AddMovieTrailerCommand, MovieTrailerDto>
{
    private readonly IMovieRepository _movieRepository;
    private readonly IMovieTrailerRepository _trailerRepository;

    public AddMovieTrailerHandler(IMovieRepository movieRepository, IMovieTrailerRepository trailerRepository)
    {
        _movieRepository = movieRepository;
        _trailerRepository = trailerRepository;
    }

    public async Task<MovieTrailerDto> Handle(AddMovieTrailerCommand request, CancellationToken cancellationToken)
    {
        var movie = await _movieRepository.GetByIdAsync(request.MovieId);
        if (movie == null) throw new KeyNotFoundException("Movie not found.");

        var trailer = new MovieTrailer
        {
            MovieId = request.MovieId,
            Title = request.Title,
            Url = request.Url,
            IsPrimary = request.IsPrimary
        };

        await _trailerRepository.AddAsync(trailer);

        return new MovieTrailerDto
        {
            Id = trailer.Id,
            Title = trailer.Title,
            Url = trailer.Url,
            IsPrimary = trailer.IsPrimary
        };
    }
}

public class DeleteMovieTrailerHandler : IRequestHandler<DeleteMovieTrailerCommand, bool>
{
    private readonly IMovieTrailerRepository _trailerRepository;

    public DeleteMovieTrailerHandler(IMovieTrailerRepository trailerRepository)
    {
        _trailerRepository = trailerRepository;
    }

    public async Task<bool> Handle(DeleteMovieTrailerCommand request, CancellationToken cancellationToken)
    {
        var trailer = await _trailerRepository.GetByIdAsync(request.TrailerId);
        if (trailer == null) throw new KeyNotFoundException("Trailer not found.");

        await _trailerRepository.DeleteAsync(trailer);
        return true;
    }
}
