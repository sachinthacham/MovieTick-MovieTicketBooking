using MediatR;
using MovieBooking.Application.DTOs.Theaters;
using MovieBooking.Application.Features.Theaters.Commands;
using MovieBooking.Application.Features.Theaters.Queries;
using MovieBooking.Application.Interfaces;
using MovieBooking.Domain.Entities;

namespace MovieBooking.Application.Features.Theaters.Handlers;

public class RateTheaterHandler : IRequestHandler<RateTheaterCommand, bool>
{
    private readonly ITheaterRatingRepository _ratingRepository;
    private readonly ITheaterRepository _theaterRepository;

    public RateTheaterHandler(ITheaterRatingRepository ratingRepository, ITheaterRepository theaterRepository)
    {
        _ratingRepository = ratingRepository;
        _theaterRepository = theaterRepository;
    }

    public async Task<bool> Handle(RateTheaterCommand request, CancellationToken cancellationToken)
    {
        if (request.Rating < 1 || request.Rating > 5)
            throw new ArgumentException("Rating must be between 1 and 5.");

        var theater = await _theaterRepository.GetByIdAsync(request.TheaterId);
        if (theater == null) throw new KeyNotFoundException("Theater not found.");

        var existing = await _ratingRepository.GetByUserAndTheaterAsync(request.UserId, request.TheaterId);
        if (existing != null)
        {
            existing.Rating = request.Rating;
            existing.Comment = request.Comment;
            existing.UpdatedAt = DateTime.UtcNow;
            await _ratingRepository.UpdateAsync(existing);
        }
        else
        {
            await _ratingRepository.AddAsync(new TheaterRating
            {
                TheaterId = request.TheaterId,
                UserId = request.UserId,
                Rating = request.Rating,
                Comment = request.Comment
            });
        }

        return true;
    }
}

public class GetTheaterRatingsHandler : IRequestHandler<GetTheaterRatingsQuery, List<TheaterRatingDto>>
{
    private readonly ITheaterRatingRepository _ratingRepository;
    private readonly IUserRepository _userRepository;

    public GetTheaterRatingsHandler(ITheaterRatingRepository ratingRepository, IUserRepository userRepository)
    {
        _ratingRepository = ratingRepository;
        _userRepository = userRepository;
    }

    public async Task<List<TheaterRatingDto>> Handle(GetTheaterRatingsQuery request, CancellationToken cancellationToken)
    {
        var ratings = await _ratingRepository.GetByTheaterAsync(request.TheaterId);
        var result = new List<TheaterRatingDto>();

        foreach (var rating in ratings)
        {
            var user = await _userRepository.GetByIdAsync(rating.UserId);
            result.Add(new TheaterRatingDto
            {
                Id = rating.Id,
                UserId = rating.UserId,
                UserName = user?.FullName ?? "Unknown",
                Rating = rating.Rating,
                Comment = rating.Comment,
                CreatedAt = rating.CreatedAt
            });
        }

        return result;
    }
}
