namespace MovieBooking.Application.DTOs.Movies;

public class MovieReviewDto
{
    public Guid Id { get; set; }
    public Guid MovieId { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; } = default!;
    public string ReviewText { get; set; } = default!;
    public bool IsApproved { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class AddMovieReviewDto
{
    public string ReviewText { get; set; } = default!;
}

public class UpdateMovieReviewDto
{
    public string ReviewText { get; set; } = default!;
}
