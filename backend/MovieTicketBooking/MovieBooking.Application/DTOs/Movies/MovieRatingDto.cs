namespace MovieBooking.Application.DTOs.Movies;

public class MovieRatingDto
{
    public Guid Id { get; set; }
    public Guid MovieId { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; } = default!;
    public decimal Rating { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class RateMovieDto
{
    public decimal Rating { get; set; }
}
