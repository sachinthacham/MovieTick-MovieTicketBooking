using Moq;
using MovieBooking.Application.Features.Movies.Commands;
using MovieBooking.Application.Features.Movies.Handlers;
using MovieBooking.Application.Interfaces;
using MovieBooking.Domain.Entities;

namespace MovieBooking.Application.Tests.Movies;

public class RateMovieHandlerTests
{
    [Fact]
    public async Task Handle_ShouldThrow_WhenRatingOutsideRange()
    {
        var ratingRepository = new Mock<IMovieRatingRepository>();
        var movieRepository = new Mock<IMovieRepository>();
        var handler = new RateMovieHandler(ratingRepository.Object, movieRepository.Object);

        var act = () => handler.Handle(new RateMovieCommand(Guid.NewGuid(), Guid.NewGuid(), 11), CancellationToken.None);

        var exception = await Assert.ThrowsAsync<ArgumentException>(act);
        Assert.Equal("Rating must be between 1 and 10.", exception.Message);
    }

    [Fact]
    public async Task Handle_ShouldAddRating_WhenNoExistingRating()
    {
        var ratingRepository = new Mock<IMovieRatingRepository>();
        var movieRepository = new Mock<IMovieRepository>();
        var movieId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var movie = new Movie
        {
            Id = movieId,
            Title = "Interstellar"
        };

        movieRepository
            .Setup(x => x.GetByIdAsync(movieId))
            .ReturnsAsync(movie);
        ratingRepository
            .Setup(x => x.GetByUserAndMovieAsync(userId, movieId))
            .ReturnsAsync((MovieRating?)null);
        ratingRepository
            .Setup(x => x.GetAverageRatingAsync(movieId))
            .ReturnsAsync(8.26m);
        ratingRepository
            .Setup(x => x.GetTotalRatingsAsync(movieId))
            .ReturnsAsync(12);

        MovieRating? addedRating = null;
        ratingRepository
            .Setup(x => x.AddAsync(It.IsAny<MovieRating>()))
            .Callback<MovieRating>(rating => addedRating = rating)
            .Returns(Task.CompletedTask);
        movieRepository
            .Setup(x => x.UpdateAsync(movie))
            .Returns(Task.CompletedTask);

        var handler = new RateMovieHandler(ratingRepository.Object, movieRepository.Object);
        var result = await handler.Handle(new RateMovieCommand(movieId, userId, 9), CancellationToken.None);

        Assert.True(result);
        Assert.NotNull(addedRating);
        Assert.Equal(movieId, addedRating!.MovieId);
        Assert.Equal(userId, addedRating.UserId);
        Assert.Equal(9, addedRating.Rating);
        Assert.Equal(8.3m, movie.AverageRating);
        Assert.Equal(12, movie.TotalRatings);
        ratingRepository.Verify(x => x.AddAsync(It.IsAny<MovieRating>()), Times.Once);
        ratingRepository.Verify(x => x.UpdateAsync(It.IsAny<MovieRating>()), Times.Never);
        movieRepository.Verify(x => x.UpdateAsync(movie), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldUpdateExistingRating_WhenAlreadyRated()
    {
        var ratingRepository = new Mock<IMovieRatingRepository>();
        var movieRepository = new Mock<IMovieRepository>();
        var movieId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var movie = new Movie
        {
            Id = movieId,
            Title = "Inception"
        };
        var existingRating = new MovieRating
        {
            Id = Guid.NewGuid(),
            MovieId = movieId,
            UserId = userId,
            Rating = 7
        };

        movieRepository
            .Setup(x => x.GetByIdAsync(movieId))
            .ReturnsAsync(movie);
        ratingRepository
            .Setup(x => x.GetByUserAndMovieAsync(userId, movieId))
            .ReturnsAsync(existingRating);
        ratingRepository
            .Setup(x => x.GetAverageRatingAsync(movieId))
            .ReturnsAsync(8.71m);
        ratingRepository
            .Setup(x => x.GetTotalRatingsAsync(movieId))
            .ReturnsAsync(22);
        ratingRepository
            .Setup(x => x.UpdateAsync(existingRating))
            .Returns(Task.CompletedTask);
        movieRepository
            .Setup(x => x.UpdateAsync(movie))
            .Returns(Task.CompletedTask);

        var handler = new RateMovieHandler(ratingRepository.Object, movieRepository.Object);
        var result = await handler.Handle(new RateMovieCommand(movieId, userId, 10), CancellationToken.None);

        Assert.True(result);
        Assert.Equal(10, existingRating.Rating);
        Assert.NotNull(existingRating.UpdatedAt);
        Assert.Equal(8.7m, movie.AverageRating);
        Assert.Equal(22, movie.TotalRatings);
        ratingRepository.Verify(x => x.UpdateAsync(existingRating), Times.Once);
        ratingRepository.Verify(x => x.AddAsync(It.IsAny<MovieRating>()), Times.Never);
        movieRepository.Verify(x => x.UpdateAsync(movie), Times.Once);
    }
}
