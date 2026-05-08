using MediatR;
using MovieBooking.Application.Features.Movies.Commands;
using MovieBooking.Application.Interfaces;
using MovieBooking.Domain.Entities;

namespace MovieBooking.Application.Features.Movies.Handlers;

public class RateMovieHandler : IRequestHandler<RateMovieCommand, bool>
{
    private readonly IMovieRatingRepository _ratingRepository;
    private readonly IMovieRepository _movieRepository;

    public RateMovieHandler(IMovieRatingRepository ratingRepository, IMovieRepository movieRepository)
    {
        _ratingRepository = ratingRepository;
        _movieRepository = movieRepository;
    }

    public async Task<bool> Handle(RateMovieCommand request, CancellationToken cancellationToken)
    {
        if (request.Rating < 1 || request.Rating > 10)
            throw new ArgumentException("Rating must be between 1 and 10.");

        var movie = await _movieRepository.GetByIdAsync(request.MovieId);
        if (movie == null) throw new KeyNotFoundException("Movie not found.");

        var existing = await _ratingRepository.GetByUserAndMovieAsync(request.UserId, request.MovieId);
        if (existing != null)
        {
            existing.Rating = request.Rating;
            existing.UpdatedAt = DateTime.UtcNow;
            await _ratingRepository.UpdateAsync(existing);
        }
        else
        {
            await _ratingRepository.AddAsync(new MovieRating
            {
                MovieId = request.MovieId,
                UserId = request.UserId,
                Rating = request.Rating
            });
        }

        var avg = await _ratingRepository.GetAverageRatingAsync(request.MovieId);
        var total = await _ratingRepository.GetTotalRatingsAsync(request.MovieId);
        movie.AverageRating = Math.Round(avg, 1);
        movie.TotalRatings = total;
        movie.UpdatedAt = DateTime.UtcNow;
        await _movieRepository.UpdateAsync(movie);

        return true;
    }
}
