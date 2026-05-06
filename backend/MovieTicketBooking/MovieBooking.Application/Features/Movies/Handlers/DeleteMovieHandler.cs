using MediatR;
using MovieBooking.Application.Features.Movies.Commands;
using MovieBooking.Application.Interfaces;

namespace MovieBooking.Application.Features.Movies.Handlers;

public class DeleteMovieHandler : IRequestHandler<DeleteMovieCommand, bool>
{
    private readonly IMovieRepository _movieRepository;

    public DeleteMovieHandler(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }

    public async Task<bool> Handle(DeleteMovieCommand request, CancellationToken cancellationToken)
    {
        var movie = await _movieRepository.GetByIdAsync(request.Id);
        if (movie == null)
            throw new KeyNotFoundException($"Movie with id {request.Id} not found.");

        await _movieRepository.DeleteAsync(movie);
        return true;
    }
}
