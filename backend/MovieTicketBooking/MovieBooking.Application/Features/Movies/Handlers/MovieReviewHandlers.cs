using MediatR;
using MovieBooking.Application.DTOs.Movies;
using MovieBooking.Application.Features.Movies.Commands;
using MovieBooking.Application.Features.Movies.Queries;
using MovieBooking.Application.Interfaces;
using MovieBooking.Domain.Entities;

namespace MovieBooking.Application.Features.Movies.Handlers;

public class AddMovieReviewHandler : IRequestHandler<AddMovieReviewCommand, MovieReviewDto>
{
    private readonly IMovieReviewRepository _reviewRepository;
    private readonly IMovieRepository _movieRepository;
    private readonly IUserRepository _userRepository;

    public AddMovieReviewHandler(IMovieReviewRepository reviewRepository, IMovieRepository movieRepository, IUserRepository userRepository)
    {
        _reviewRepository = reviewRepository;
        _movieRepository = movieRepository;
        _userRepository = userRepository;
    }

    public async Task<MovieReviewDto> Handle(AddMovieReviewCommand request, CancellationToken cancellationToken)
    {
        var movie = await _movieRepository.GetByIdAsync(request.MovieId);
        if (movie == null) throw new KeyNotFoundException("Movie not found.");

        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null) throw new KeyNotFoundException("User not found.");

        var review = new MovieReview
        {
            MovieId = request.MovieId,
            UserId = request.UserId,
            ReviewText = request.ReviewText,
            IsApproved = false
        };

        await _reviewRepository.AddAsync(review);

        return new MovieReviewDto
        {
            Id = review.Id,
            MovieId = review.MovieId,
            UserId = review.UserId,
            UserName = user.FullName,
            ReviewText = review.ReviewText,
            IsApproved = review.IsApproved,
            CreatedAt = review.CreatedAt
        };
    }
}

public class UpdateMovieReviewHandler : IRequestHandler<UpdateMovieReviewCommand, MovieReviewDto>
{
    private readonly IMovieReviewRepository _reviewRepository;
    private readonly IUserRepository _userRepository;

    public UpdateMovieReviewHandler(IMovieReviewRepository reviewRepository, IUserRepository userRepository)
    {
        _reviewRepository = reviewRepository;
        _userRepository = userRepository;
    }

    public async Task<MovieReviewDto> Handle(UpdateMovieReviewCommand request, CancellationToken cancellationToken)
    {
        var review = await _reviewRepository.GetByIdAsync(request.ReviewId);
        if (review == null) throw new KeyNotFoundException("Review not found.");
        if (review.UserId != request.UserId) throw new UnauthorizedAccessException("Cannot edit another user's review.");

        review.ReviewText = request.ReviewText;
        review.UpdatedAt = DateTime.UtcNow;
        await _reviewRepository.UpdateAsync(review);

        var user = await _userRepository.GetByIdAsync(review.UserId);

        return new MovieReviewDto
        {
            Id = review.Id,
            MovieId = review.MovieId,
            UserId = review.UserId,
            UserName = user?.FullName ?? string.Empty,
            ReviewText = review.ReviewText,
            IsApproved = review.IsApproved,
            CreatedAt = review.CreatedAt,
            UpdatedAt = review.UpdatedAt
        };
    }
}

public class DeleteMovieReviewHandler : IRequestHandler<DeleteMovieReviewCommand, bool>
{
    private readonly IMovieReviewRepository _reviewRepository;

    public DeleteMovieReviewHandler(IMovieReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    public async Task<bool> Handle(DeleteMovieReviewCommand request, CancellationToken cancellationToken)
    {
        var review = await _reviewRepository.GetByIdAsync(request.ReviewId);
        if (review == null) throw new KeyNotFoundException("Review not found.");
        if (!request.IsAdmin && review.UserId != request.UserId)
            throw new UnauthorizedAccessException("Cannot delete another user's review.");

        await _reviewRepository.DeleteAsync(review);
        return true;
    }
}

public class ApproveMovieReviewHandler : IRequestHandler<ApproveMovieReviewCommand, bool>
{
    private readonly IMovieReviewRepository _reviewRepository;

    public ApproveMovieReviewHandler(IMovieReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    public async Task<bool> Handle(ApproveMovieReviewCommand request, CancellationToken cancellationToken)
    {
        var review = await _reviewRepository.GetByIdAsync(request.ReviewId);
        if (review == null) throw new KeyNotFoundException("Review not found.");

        review.IsApproved = true;
        review.UpdatedAt = DateTime.UtcNow;
        await _reviewRepository.UpdateAsync(review);
        return true;
    }
}

public class GetMovieReviewsHandler : IRequestHandler<GetMovieReviewsQuery, List<MovieReviewDto>>
{
    private readonly IMovieReviewRepository _reviewRepository;
    private readonly IUserRepository _userRepository;

    public GetMovieReviewsHandler(IMovieReviewRepository reviewRepository, IUserRepository userRepository)
    {
        _reviewRepository = reviewRepository;
        _userRepository = userRepository;
    }

    public async Task<List<MovieReviewDto>> Handle(GetMovieReviewsQuery request, CancellationToken cancellationToken)
    {
        var reviews = await _reviewRepository.GetByMovieAsync(request.MovieId, request.ApprovedOnly);

        var result = new List<MovieReviewDto>();
        foreach (var review in reviews)
        {
            var user = await _userRepository.GetByIdAsync(review.UserId);
            result.Add(new MovieReviewDto
            {
                Id = review.Id,
                MovieId = review.MovieId,
                UserId = review.UserId,
                UserName = user?.FullName ?? "Unknown",
                ReviewText = review.ReviewText,
                IsApproved = review.IsApproved,
                CreatedAt = review.CreatedAt,
                UpdatedAt = review.UpdatedAt
            });
        }

        return result;
    }
}
