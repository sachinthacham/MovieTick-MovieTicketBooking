using MediatR;
using MovieBooking.Application.DTOs.Movies;
using MovieBooking.Application.Features.Movies.Queries;
using MovieBooking.Application.Interfaces;

namespace MovieBooking.Application.Features.Movies.Handlers;

public class GetMovieByIdHandler : IRequestHandler<GetMovieByIdQuery, MovieDto>
{
    private readonly IMovieRepository _movieRepository;

    public GetMovieByIdHandler(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }

    public async Task<MovieDto> Handle(GetMovieByIdQuery request, CancellationToken cancellationToken)
    {
        var movie = await _movieRepository.GetByIdAsync(request.Id);
        if (movie == null)
            throw new KeyNotFoundException($"Movie with id {request.Id} not found.");

        return MovieMappingHelper.ToDto(movie);
    }
}
