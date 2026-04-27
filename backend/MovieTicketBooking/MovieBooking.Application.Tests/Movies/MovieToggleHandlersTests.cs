using Moq;
using MovieBooking.Application.Features.Movies.Commands;
using MovieBooking.Application.Features.Movies.Handlers;
using MovieBooking.Application.Interfaces;
using MovieBooking.Domain.Entities;

namespace MovieBooking.Application.Tests.Movies;

public class MovieToggleHandlersTests
{
    [Fact]
    public async Task ToggleFeatured_ShouldThrow_WhenMovieDoesNotExist()
    {
        var movieRepository = new Mock<IMovieRepository>();
        var movieId = Guid.NewGuid();

        movieRepository
            .Setup(x => x.GetByIdAsync(movieId))
            .ReturnsAsync((Movie?)null);

        var handler = new ToggleFeaturedHandler(movieRepository.Object);
        var act = () => handler.Handle(new ToggleFeaturedCommand(movieId, true), CancellationToken.None);

        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(act);
        Assert.Contains(movieId.ToString(), exception.Message);
    }

    [Fact]
    public async Task ToggleFeatured_ShouldUpdateMovie_WhenMovieExists()
    {
        var movieRepository = new Mock<IMovieRepository>();
        var movie = new Movie
        {
            Id = Guid.NewGuid(),
            Title = "Dune",
            IsFeatured = false
        };

        movieRepository
            .Setup(x => x.GetByIdAsync(movie.Id))
            .ReturnsAsync(movie);
        movieRepository
            .Setup(x => x.UpdateAsync(movie))
            .Returns(Task.CompletedTask);

        var handler = new ToggleFeaturedHandler(movieRepository.Object);
        var result = await handler.Handle(new ToggleFeaturedCommand(movie.Id, true), CancellationToken.None);

        Assert.True(result);
        Assert.True(movie.IsFeatured);
        AssertMovieUpdatedAt(movie);
        movieRepository.Verify(x => x.UpdateAsync(movie), Times.Once);
    }

    [Fact]
    public async Task ToggleComingSoon_ShouldUpdateMovie_WhenMovieExists()
    {
        var movieRepository = new Mock<IMovieRepository>();
        var movie = new Movie
        {
            Id = Guid.NewGuid(),
            Title = "Avatar 3",
            IsComingSoon = false
        };

        movieRepository
            .Setup(x => x.GetByIdAsync(movie.Id))
            .ReturnsAsync(movie);
        movieRepository
            .Setup(x => x.UpdateAsync(movie))
            .Returns(Task.CompletedTask);

        var handler = new ToggleComingSoonHandler(movieRepository.Object);
        var result = await handler.Handle(new ToggleComingSoonCommand(movie.Id, true), CancellationToken.None);

        Assert.True(result);
        Assert.True(movie.IsComingSoon);
        AssertMovieUpdatedAt(movie);
        movieRepository.Verify(x => x.UpdateAsync(movie), Times.Once);
    }

    private static void AssertMovieUpdatedAt(Movie movie)
    {
        Assert.NotNull(movie.UpdatedAt);
        Assert.True(movie.UpdatedAt <= DateTime.UtcNow);
    }
}
